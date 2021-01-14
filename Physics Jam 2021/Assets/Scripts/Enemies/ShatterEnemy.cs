using UnityEngine;

public class ShatterEnemy : Enemy
{
    [SerializeField] ShatterProjectile ProjectilePrefab;

    public override EnemyType EnemyType => EnemyType.Shatter;

    protected override void OnTouchedGround()
    {
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