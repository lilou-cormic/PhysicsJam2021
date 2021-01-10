public class StandardEnemy : Enemy
{
    public override EnemyType EnemyType => EnemyType.Standard;

    private void Update()
    {
        if (IsGrounded && rb.gravityScale == transform.localScale.y)
            MoveController.Move(transform, Direction, 1);
    }
}
