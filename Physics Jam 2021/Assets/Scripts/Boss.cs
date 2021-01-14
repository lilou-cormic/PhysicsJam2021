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

    private int _hitCounter = 0;
    private bool IsHit => _hitCounter > 0;
    private bool _isAngry = false;
    private bool _isAttacking = false;
    private bool _isDead = false;

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

        InvokeRepeating(nameof(Attack), 2, 2);

        rb.velocity = Vector2.left * 3 * (Random.Range(0, 2) > 0 ? 1 : -1);
    }

    private void Update()
    {
        if (_isDead)
            return;

        if (IsHit)
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
        if (_isDead)
            return;

        Enemy enemy = collision.GetComponentInParent<Enemy>();

        if (enemy != null && enemy.HasLeftBoss)
        {
            if (collision.CompareTag(GameManager.SpikesTag))
                Health.ChangeHP(-1);

            enemy.Kill();
        }

        if (_isProcessing)
            return;

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
        _hitCounter++;

        GameManager.ShakeCamera();

        yield return new WaitForSeconds(0.5f);

        _hitCounter--;
    }

    private void Attack()
    {
        if (_isDead || !gameObject.activeSelf)
            return;

        StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        if (!IsHit)
        {
            _isAngry = true;

            yield return new WaitForSeconds(0.5f);

            _isAngry = false;
            _isAttacking = true;

            if (GameManager.CurrentLevel.GravityWave && Random.Range(0, 100) < 10)
            {
                GameManager.SetGravity(GameManager.Gravity * -1);
            }
            else
            {
                var enemyType = GameManager.CurrentLevel.EnemyTypes.GetRandom();

                var enemy = EnemyPool.Current.GetItem(enemyType);
                enemy.transform.position = SpawnPoint.position;
            }

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
        if (_isDead)
            return;

        _isDead = true;

        rb.velocity = Vector2.zero;

        SpriteRenderer.sprite = HitImage;

        GameManager.Win();
    }
}
