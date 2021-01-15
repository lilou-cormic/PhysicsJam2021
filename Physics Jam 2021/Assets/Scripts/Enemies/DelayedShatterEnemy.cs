using System.Collections;
using UnityEngine;

public class DelayedShatterEnemy : ShatterEnemy
{
    public override EnemyType EnemyType => EnemyType.DelayedShatter;

    protected override void OnTouchedGround()
    {
        if (!IsDead)
            StartCoroutine(DoExplode());
    }

    private IEnumerator DoExplode()
    {
        yield return new WaitForSeconds(0.5f);

        IsExploding = true;

        yield return new WaitForSeconds(0.5f);

        Explode();
    }

    protected override void SetAsInUseInternal()
    {
        base.SetAsInUseInternal();

        IsExploding = false;
    }
}