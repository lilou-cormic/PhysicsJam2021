using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody2D))]
public class Boss : MonoBehaviour
{
    private Rigidbody2D rb = null;

    private Health Health = null;

    [SerializeField] Transform SpawnPoint = null;

    [SerializeField] SpriteRenderer SpriteRenderer = null;

    [SerializeField] Sprite NormalImage = null;
    [SerializeField] Sprite AngryImage = null;
    [SerializeField] Sprite AttackImage = null;
    [SerializeField] Sprite HitImage = null;

    private bool _isProcessing = false;

    private bool _isHit = false;
    public bool _isAngry = false;
    public bool _isAttacking = false;

    private int _direction = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Health = GetComponent<Health>();

        Health.HPChanged += Health_HPChanged;
        Health.HPDepleted += Health_HPDepleted;
    }

    private void Start()
    {
        Health.MaxHP = GameManager.CurrentLevel.BossHP;

        InvokeRepeating(nameof(SpawnEnemy), 2, 4);

        rb.velocity = Vector2.left * 3;
    }

    private void Update()
    {
        if (_isHit)
            SpriteRenderer.sprite = HitImage;
        else if (_isAttacking)
            SpriteRenderer.sprite = AttackImage;
        else if (_isAngry)
            SpriteRenderer.sprite = AngryImage;
        else
            SpriteRenderer.sprite = NormalImage;

        SpriteRenderer.gameObject.transform.localPosition = new Vector3(0, Mathf.Sin(Time.timeSinceLevelLoad) * 0.2f, 0);
    }

    private void OnDestroy()
    {
        Health.HPChanged -= Health_HPChanged;
        Health.HPDepleted -= Health_HPDepleted;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isProcessing)
            return;

        Enemy enemy = collision.GetComponentInParent<Enemy>();

        if (enemy != null && enemy.HasLeftBoss)
        {
            bool canSpawnHealth = true;

            if (!_isHit)
            {
                if (collision.CompareTag(GameManager.SpikesTag))
                {
                    Health.ChangeHP(-1);
                    canSpawnHealth = false;
                }
            }

            enemy.Kill(canSpawnHealth);
        }

        if (collision.CompareTag(GameManager.WallTag))
        {
            _isProcessing = true;

            rb.velocity *= -1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponentInParent<Enemy>()?.SetOutOfBoss();

        _isProcessing = false;
    }

    private IEnumerator DoOnDamaged()
    {
        _isHit = true;

        yield return new WaitForSeconds(0.5f);

        _isHit = false;
    }

    private void SpawnEnemy()
    {
        if (!gameObject.activeSelf)
            return;

        StartCoroutine(DoSpawnEnemy());
    }

    private IEnumerator DoSpawnEnemy()
    {
        if (!_isHit)
        {
            _isAngry = true;

            yield return new WaitForSeconds(0.5f);

            _isAngry = false;
            _isAttacking = true;

            var enemyType = GameManager.CurrentLevel.EnemyTypes.GetRandom();

            var enemy = EnemyPool.Current.GetItem(enemyType);
            enemy.transform.position = SpawnPoint.position;

            yield return new WaitForSeconds(0.5f);

            _isAttacking = false;
        }
    }

    private void Health_HPChanged(int amount)
    {
        if (amount < 0)
            StartCoroutine(DoOnDamaged());
    }

    private void Health_HPDepleted(Health health)
    {
        _isHit = true;

        GameManager.Win();
    }
}