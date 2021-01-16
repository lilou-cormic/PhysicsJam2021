using System.Collections;
using UnityEngine;

public class FlipEnemy : Enemy
{
    public override EnemyType EnemyType => EnemyType.Flip;

    protected override bool AffectedByGravitySwitch => true;

    private bool _isMoving = true;

    private void Update()
    {
        if (IsGrounded && rb.gravityScale != transform.localScale.y)
        {
            if (_isMoving)
            {
                SpriteRenderer.sprite = NormalImage;
                MoveController.Move(transform, Direction, 2);
            }
            else
            {
                SpriteRenderer.sprite = WalkImage;
            }
        }
        else
        {
            SpriteRenderer.sprite = FrownImage;
        }
    }

    protected override void Metronome_OnTick()
    {
        if (gameObject.activeSelf)
            StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        _isMoving = true;

        yield return new WaitForSeconds(0.2f);

        _isMoving = false;
    }

    protected override void SetAsInUseInternal()
    {
        base.SetAsInUseInternal();

        _isMoving = false;
    }
}
