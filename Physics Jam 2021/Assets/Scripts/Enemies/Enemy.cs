using PurpleCable;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour, IPoolable
{
    private Rigidbody2D rb = null;

    private Health Health = null;

    public abstract EnemyType EnemyType { get; }

    private bool _isDead = false;

    public bool HasTouchedGround { get; set; } = false;

    public int Direction = 1;

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

    private bool _isProcessing = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isProcessing)
            return;

        if (collision.gameObject.CompareTag(GameManager.WallTag))
            FlipDirection();

        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            if (rb.gravityScale == 1 && collision.gameObject.CompareTag(GameManager.FloorTag)
                || (rb.gravityScale == -1 && collision.gameObject.CompareTag(GameManager.CeilingTag)))
            {
                HasTouchedGround = true;

                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isProcessing = false;
    }

    public void SetGravity(float gravity)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;

        rb.gravityScale = gravity;
    }

    public void SetOutOfBoss()
    {
        HasTouchedGround = true;
    }

    private void FlipDirection()
    {
        if (_isProcessing)
            return;

        _isProcessing = true;

        Direction *= -1;
    }

    public void Damage()
    {
        if (_isDead)
            return;

        Health.ChangeHP(-1);
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
        HasTouchedGround = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = GameManager.Gravity;
        Health.FillHP();
        _isDead = false;

        gameObject.SetActive(true);
    }

    void IPoolable.SetAsAvailable()
       => gameObject.SetActive(false);
}