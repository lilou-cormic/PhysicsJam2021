public class FlipEnemy : Enemy
{
    public override EnemyType EnemyType => EnemyType.Flip;

    private void Update()
    {
        if (IsGrounded && rb.gravityScale != transform.localScale.y)
            MoveController.Move(transform, Direction, 1);
    }
}
