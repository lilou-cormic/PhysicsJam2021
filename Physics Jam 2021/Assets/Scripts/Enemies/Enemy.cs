using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour, IPoolable
{
    protected Rigidbody2D rb { get; private set; }

    protected Health Health { get; private set; }

    [SerializeField] protected SpriteRenderer SpriteRenderer = null;

    public abstract EnemyType EnemyType { get; }

    private bool _isDead = false;

    public bool HasLeftBoss { get; set; } = false;

    public bool IsGrounded { get; private set; } = false;

    public int Direction = 1;

    private bool _isProcessing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Health = GetComponent<Health>();

        Health.HPDepleted += Health_HPDepleted;
    }

    private void OnDestroy()
    {
        Health.HPDepleted -= Health_HPDepleted;
    }

    private void OnEnable()
    {
        StartCoroutine(DoAppear());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isProcessing)
            return;

        if (collision.gameObject.CompareTag(GameManager.WallTag))
            FlipDirection();

        if (rb.gravityScale == 1 && collision.gameObject.CompareTag(GameManager.FloorTag)
            || (rb.gravityScale == -1 && collision.gameObject.CompareTag(GameManager.CeilingTag)))
        {
            IsGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameManager.FloorTag) || collision.gameObject.CompareTag(GameManager.CeilingTag))
            IsGrounded = false;

        _isProcessing = false;
    }

    private IEnumerator DoAppear()
    {
        for (float i = 0; i < 1f; i += 0.1f)
        {
            SpriteRenderer.gameObject.transform.localScale = new Vector3(i, i, i);

            yield return new WaitForSeconds(0.05f);
        }

        SpriteRenderer.gameObject.transform.localScale = Vector3.one;
    }

    public void SetGravity(float gravity)
    {
        rb.gravityScale = gravity;
    }

    public void SetOutOfBoss()
    {
        HasLeftBoss = true;
    }

    private void FlipDirection()
    {
        if (_isProcessing)
            return;

        _isProcessing = true;

        Direction *= -1;
    }

    public void Kill(bool canSpawnHealth)
    {
        if (_isDead)
            return;

        Health.ChangeHP(-1);

        if (canSpawnHealth && !GameManager.Player.HasFullHealth)
        {
            if (Random.Range(0, 100) <= 25)
            {
                var healthPickup = HealthPickupPool.Current.GetItem();
                healthPickup.transform.position = transform.position;
            }
        }
    }

    private void Health_HPDepleted(Health health)
    {
        if (_isDead)
            return;

        _isDead = true;

        ((IPoolable)(this)).SetAsAvailable();
    }

    bool IPoolable.IsInUse
        => gameObject.activeSelf;

    void IPoolable.SetAsInUse()
    {
        Direction = (Random.Range(0, 2) > 0 ? 1 : -1);
        HasLeftBoss = false;
        IsGrounded = false;
        rb.gravityScale = GameManager.Gravity;
        transform.localScale = new Vector3(1, GameManager.Gravity, 1);
        Health.FillHP();
        _isDead = false;

        gameObject.SetActive(true);
    }

    void IPoolable.SetAsAvailable()
       => gameObject.SetActive(false);
}