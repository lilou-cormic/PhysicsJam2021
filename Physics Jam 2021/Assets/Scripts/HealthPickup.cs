using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HealthPickup : PoolableItem
{
    protected Rigidbody2D rb { get; private set; }

    [SerializeField] SpriteRenderer SpriteRenderer = null;

    [SerializeField] Color PopColor = Color.white;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(DoAppear());
    }

    protected override void Init()
    {
        rb.gravityScale = GameManager.Gravity;
        transform.localScale = new Vector3(1, GameManager.Gravity, 1);
    }

    protected override bool CanPickup(Collider2D collision)
    {
        return collision.CompareTag("Player");
    }

    protected override void OnPickup(Collider2D collision)
    {
        collision.GetComponent<Health>().ChangeHP(1);

        PopPool.ShowPop(PopColor, transform.position, playSound: false);
    }

    public void SetGravity(float gravity)
    {
        rb.gravityScale = gravity;
    }

    private IEnumerator DoAppear()
    {
        int y = (transform.localScale.y >= 0 ? 1 : -1);

        for (float i = 0; i < 1f; i += 0.1f)
        {
            SpriteRenderer.gameObject.transform.localPosition = new Vector3(0, (i <= 0.5f ? i : 1 - i), 0);
            SpriteRenderer.gameObject.transform.localScale = new Vector3(i, i, i);

            yield return new WaitForSeconds(0.01f);
        }

        SpriteRenderer.gameObject.transform.localPosition = Vector3.zero;
        SpriteRenderer.gameObject.transform.localScale = Vector3.one;
    }
}
