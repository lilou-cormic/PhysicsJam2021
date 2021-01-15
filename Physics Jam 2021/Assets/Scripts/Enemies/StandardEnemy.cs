public class StandardEnemy : Enemy
{
    public override EnemyType EnemyType => EnemyType.Standard;

    protected override bool AffectedByGravitySwitch => true;
}
