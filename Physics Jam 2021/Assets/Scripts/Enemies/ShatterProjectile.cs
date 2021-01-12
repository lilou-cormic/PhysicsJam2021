using PurpleCable;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShatterProjectile : MonoBehaviour
{
    protected Rigidbody2D rb { get; private set; }

    private bool _isDestroyed = false;

    public int Direction { get; set; } = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveController.Move(transform, Direction, 5);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDestroyed)
            return;

        _isDestroyed = true;

        collision.gameObject.GetComponent<Health>()?.ChangeHP(-1);

        Destroy(gameObject);
    }
}