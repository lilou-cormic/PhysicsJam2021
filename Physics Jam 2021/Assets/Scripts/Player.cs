using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb = null;

    private Health Health = null;

    private Animator Animator = null;

    [SerializeField] AudioSource HitAudioSource = null;

    [SerializeField] AudioClip AttackSound = null;

    [SerializeField] SpriteRenderer SpriteRenderer = null;

    [SerializeField] Sprite NormalImage = null;
    [SerializeField] Sprite HitImage = null;
    [SerializeField] Sprite AttackImage = null;

    public bool IsGrounded { get; private set; } = false;

    public bool HasFullHealth => Health.CurrentHP == Health.MaxHP;

    private bool _isProcessing = false;

    private bool _isHit = false;
    private bool _isAttacking = false;

    private bool _isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Animator = GetComponent<Animator>();

        Health = GetComponent<Health>();
        Health.HPChanged += Health_HPChanged;
        Health.HPDepleted += Health_HPDepleted;
    }

    private void OnDestroy()
    {
        Health.HPChanged -= Health_HPChanged;
        Health.HPDepleted -= Health_HPDepleted;
    }

    private void Update()
    {
        if (_isDead)
            return;

        if (_isHit)
            SpriteRenderer.sprite = HitImage;
        else if (_isAttacking)
            SpriteRenderer.sprite = AttackImage;
        else
            SpriteRenderer.sprite = NormalImage;

        if (!IsGrounded)
        {
            Animator.SetFloat("speed", 0);

            return;
        }

        var horizontal = Input.GetAxisRaw("Horizontal");

        MoveController.Move(transform, horizontal, 10);

        Animator.SetFloat("speed", Mathf.Abs(horizontal));

        if (Input.GetButtonDown("Fire1"))
            StartCoroutine(DoAttack());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isProcessing)
            return;

        if (rb.gravityScale == 1 && collision.gameObject.CompareTag(GameManager.FloorTag)
            || (rb.gravityScale == -1 && collision.gameObject.CompareTag(GameManager.CeilingTag)))
        {
            IsGrounded = true;
        }

        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            Health.ChangeHP(-1);

            enemy.Kill();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameManager.FloorTag) || collision.gameObject.CompareTag(GameManager.CeilingTag))
            IsGrounded = false;

        _isProcessing = false;
    }

    public void SetGravity(float gravity)
    {
        IsGrounded = false;

        rb.gravityScale = gravity;

        StartCoroutine(DoSetGravity());
    }

    private IEnumerator DoSetGravity()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        yield return new WaitForSeconds(0.4f);

        for (float i = -1; i < 1; i += 0.1f)
        {
            transform.localScale = new Vector3(transform.localScale.x, i * rb.gravityScale, transform.localScale.z);

            yield return new WaitForSeconds(0.01f);
        }

        transform.localScale = new Vector3(transform.localScale.x, rb.gravityScale, transform.localScale.z);
    }

    private IEnumerator DoAttack()
    {

        Animator.SetTrigger("attack");

        _isAttacking = true;

        AttackSound.PlayRandomPitch();

        yield return new WaitForSeconds(0.2f);

        _isAttacking = false;
    }

    private IEnumerator DoOnDamaged()
    {
        _isHit = true;

        HitAudioSource.volume = SoundPlayer.Volume;
        HitAudioSource.Play();

        GameManager.ShakeCamera();

        yield return new WaitForSeconds(0.5f);

        _isHit = false;
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

        Animator.SetFloat("speed", 0);

        SpriteRenderer.sprite = HitImage;

        GameManager.GameOver();
    }
}