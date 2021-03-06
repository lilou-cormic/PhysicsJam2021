using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour, IPoolable
{
    protected Rigidbody2D rb { get; private set; }

    [SerializeField] protected SpriteRenderer SpriteRenderer = null;

    [SerializeField] protected Sprite NormalImage = null;
    [SerializeField] protected Sprite WalkImage = null;
    [SerializeField] protected Sprite FrownImage = null;

    [SerializeField] Color PopColor = Color.white;

    public abstract EnemyType EnemyType { get; }

    protected abstract bool AffectedByGravitySwitch { get; }

    private bool _isDead = false;
    protected bool IsDead => _isDead;

    public bool HasLeftBoss { get; set; } = false;

    private bool _IsGrounded = false;
    public bool IsGrounded
    {
        get => _IsGrounded;

        set
        {
            if (_IsGrounded != value)
            {
                _IsGrounded = value;

                if (_IsGrounded)
                    OnTouchedGround();
            }
        }
    }

    protected int Direction = 1;

    private bool _isProcessing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Metronome.Current.OnTick += Metronome_OnTick;
    }

    private void OnDestroy()
    {
        Metronome.Current.OnTick -= Metronome_OnTick;
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

    protected virtual void OnTouchedGround()
    { }

    public void SetGravity(float gravity)
    {
        if (!gameObject.activeSelf || AffectedByGravitySwitch)
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

    public void Kill()
    {
        _isDead = true;

        PopPool.ShowPop(PopColor, transform.position);

        ((IPoolable)(this)).SetAsAvailable();
    }

    protected virtual void Metronome_OnTick()
    { }

    public void SetSortingOrder(int sortingOrder)
    {
        SpriteRenderer.sortingOrder = sortingOrder;
    }

    bool IPoolable.IsInUse
        => gameObject.activeSelf;

    void IPoolable.SetAsInUse()
    {
        Direction = (Random.Range(0, 2) > 0 ? 1 : -1);
        HasLeftBoss = false;
        IsGrounded = false;
        rb.gravityScale = GameManager.Gravity;
        transform.localScale = new Vector3(Direction, GameManager.Gravity, 1);
        _isDead = false;

        SetAsInUseInternal();

        gameObject.SetActive(true);
    }

    protected virtual void SetAsInUseInternal()
    { }

    void IPoolable.SetAsAvailable()
       => gameObject.SetActive(false);
}
