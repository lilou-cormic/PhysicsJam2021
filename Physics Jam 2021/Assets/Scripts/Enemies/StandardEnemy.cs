using UnityEngine;

public class StandardEnemy : Enemy
{
    public override EnemyType EnemyType => EnemyType.Standard;

    [SerializeField] Sprite NormalImage = null;
    [SerializeField] Sprite FrownImage = null;

    private void Update()
    {
        if (IsGrounded && rb.gravityScale == transform.localScale.y)
        {
            SpriteRenderer.sprite = NormalImage;
            MoveController.Move(transform, Direction, 1);
        }
        else
        {
            SpriteRenderer.sprite = FrownImage;
        }

    }
}
