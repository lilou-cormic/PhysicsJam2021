﻿using UnityEngine;

public class FlipEnemy : Enemy
{
    public override EnemyType EnemyType => EnemyType.Flip;

    [SerializeField] Sprite NormalImage = null;
    [SerializeField] Sprite FrownImage = null;

    private void Update()
    {
        if (IsGrounded && rb.gravityScale != transform.localScale.y)
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
