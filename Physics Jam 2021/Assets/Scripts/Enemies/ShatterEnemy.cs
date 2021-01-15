using UnityEngine;

public class ShatterEnemy : Enemy
{
    [SerializeField] ShatterProjectile ProjectilePrefab;

    public override EnemyType EnemyType => EnemyType.Shatter;

    protected override bool AffectedByGravitySwitch => false;

    protected bool IsExploding { get; set; } = false;

    protected override void Update()
    {
        if (IsGrounded && rb.gravityScale == transform.localScale.y && !IsExploding)
        {
            SpriteRenderer.sprite = NormalImage;
            MoveController.Move(transform, Direction, 1);
        }
        else
        {
            SpriteRenderer.sprite = FrownImage;
        }

    }

    protected override void OnTouchedGround()
    {
        IsExploding = true;

        Explode();
    }

    protected void Explode()
    {
        if (IsDead)
            return;

        var projectilePrefab = Instantiate(ProjectilePrefab, transform.parent);
        projectilePrefab.transform.position = transform.position;
        projectilePrefab.Direction = 1;

        projectilePrefab = Instantiate(ProjectilePrefab, transform.parent);
        projectilePrefab.transform.position = transform.position;
        projectilePrefab.Direction = -1;

        Kill();
    }

    
}