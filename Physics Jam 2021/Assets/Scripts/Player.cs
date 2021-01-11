using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb = null;

    private Health Health = null;

    [SerializeField] SpriteRenderer SpriteRenderer = null;

    [SerializeField] PlayerSlash Slash = null;

    public bool IsGrounded { get; private set; } = false;

    public bool HasFullHealth => Health.CurrentHP == Health.MaxHP;

    private bool _isProcessing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

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
        if (!IsGrounded)
            return;

        MoveController.Move(transform, Input.GetAxisRaw("Horizontal"), 10);

        if (Input.GetButtonDown("Fire1"))
            StartCoroutine(DoSlash());
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

            enemy.Kill(false);
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

        yield return new WaitForSeconds(0.1f);

        for (float i = -1; i < 1; i += 0.1f)
        {
            transform.localScale = new Vector3(transform.localScale.x, i * rb.gravityScale, transform.localScale.z);

            yield return new WaitForSeconds(0.01f);
        }

        transform.localScale = new Vector3(transform.localScale.x, rb.gravityScale, transform.localScale.z);
    }

    private IEnumerator DoSlash()
    {
        Slash.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        Slash.gameObject.SetActive(false);
    }

    private IEnumerator DoOnDamaged()
    {
        var color = SpriteRenderer.color;

        SpriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        SpriteRenderer.color = color;
    }

    private void Health_HPChanged(int amount)
    {
        if (amount < 0)
            StartCoroutine(DoOnDamaged());
    }

    private void Health_HPDepleted(Health health)
    {
        GameManager.GameOver();
    }
}