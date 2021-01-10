using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody2D))]
public class Boss : MonoBehaviour
{
    private Rigidbody2D rb = null;

    private Health Health = null;

    [SerializeField] SpriteRenderer SpriteRenderer = null;

    private bool _isProcessing = false;

    private int _direction = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Health = GetComponent<Health>();

        Health.MaxHP = GameManager.CurrentLevel.BossHP;

        Health.HPChanged += Health_HPChanged;
        Health.HPDepleted += Health_HPDepleted;
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2, 5);

        rb.velocity = Vector2.left * 2;
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

        if (enemy != null && enemy.HasTouchedGround)
        {
            if (collision.CompareTag(GameManager.SpikesTag))
                Health.ChangeHP(-1);

            enemy.Damage();
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
        SpriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        SpriteRenderer.color = Color.white;
    }

    private void SpawnEnemy()
    {
        if (!gameObject.activeSelf)
            return;

        var enemyType = GameManager.CurrentLevel.EnemyTypes.GetRandom();

        var enemy = EnemyPool.Current.GetItem(enemyType);
        enemy.transform.position = transform.position;
    }

    private void Health_HPChanged(Health health)
    {
        StartCoroutine(DoOnDamaged());
    }

    private void Health_HPDepleted(Health health)
    {
        GameManager.Win();
    }
}