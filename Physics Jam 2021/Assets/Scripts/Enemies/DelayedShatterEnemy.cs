using System.Collections;
using UnityEngine;

public class DelayedShatterEnemy : ShatterEnemy
{
    public override EnemyType EnemyType => EnemyType.DelayedShatter;

    protected override void OnTouchedGround()
    {
        StartCoroutine(DoExplode());
    }

    private IEnumerator DoExplode()
    {
        yield return new WaitForSeconds(1f);

        Explode();
    }
}