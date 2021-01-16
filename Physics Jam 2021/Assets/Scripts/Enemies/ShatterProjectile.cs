using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShatterProjectile : MonoBehaviour
{
    protected Rigidbody2D rb { get; private set; }

    [SerializeField] SpriteRenderer SpriteRenderer = null;

    private bool _isDestroyed = false;

    public int Direction { get; set; } = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(DoAppear());
    }

    private void Update()
    {
        MoveController.Move(transform, Direction, 5.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDestroyed)
            return;

        _isDestroyed = true;

        collision.gameObject.GetComponent<Health>()?.ChangeHP(-1);

        Destroy(gameObject);
    }

    private IEnumerator DoAppear()
    {
        for (float i = 0; i < 1f; i += 0.1f)
        {
            SpriteRenderer.gameObject.transform.localScale = new Vector3(i, i, i);

            yield return new WaitForSeconds(0.03f);
        }

        SpriteRenderer.gameObject.transform.localScale = Vector3.one;
    }
}