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

    public bool IsGrounded { get; private set; } = false;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isProcessing)
            return;

        if (rb.gravityScale == 1 && collision.gameObject.CompareTag(GameManager.FloorTag)
                || (rb.gravityScale == -1 && collision.gameObject.CompareTag(GameManager.CeilingTag)))
        {
            _isProcessing = true;
            IsGrounded = true;
        }

        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            Health.ChangeHP(-1);

            enemy.Damage();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameManager.FloorTag) || collision.gameObject.CompareTag(GameManager.CeilingTag))
            IsGrounded = false;

        _isProcessing = false;
    }

    private void Update()
    {
        if (!IsGrounded)
            return;

        //rb.velocity = Vector2.right * Input.GetAxisRaw("Horizontal") * 1000 * Time.deltaTime;
        //rb.AddForce(Vector2.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime, ForceMode2D.Impulse);
        transform.Translate(Input.GetAxisRaw("Horizontal") * 10 * Time.deltaTime, 0, 0, transform);
    }

    public void SetGravity(float gravity)
    {
        IsGrounded = false;

        rb.gravityScale = gravity;
    }

    private IEnumerator DoOnDamaged()
    {
        var color = SpriteRenderer.color;

        SpriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        SpriteRenderer.color = color;
    }

    private void Health_HPChanged(Health health)
    {
        StartCoroutine(DoOnDamaged());
    }

    private void Health_HPDepleted(Health health)
    {
        GameManager.GameOver();
    }
}
