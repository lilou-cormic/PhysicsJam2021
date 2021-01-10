using PurpleCable;
using System;
using System.Collections.Generic;

public sealed class EnemyPool : Pool<Enemy, EnemyType>
{
    public static EnemyPool Current { get; private set; } = null;

    protected override void Awake()
    {
        base.Awake();

        Current = this;
    }

    protected override Dictionary<EnemyType, List<Enemy>> GetInitialLists()
    {
        var lists = new Dictionary<EnemyType, List<Enemy>>();

        foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
        {
            lists.Add(enemyType, new List<Enemy>(BatchCount));
        }

        return lists;
    }

    protected override Enemy CreateItem(EnemyType category)
    {
        Enemy enemy = Instantiate(GameManager.GetEnemyPrefab(category), transform);
        ((IPoolable)enemy).SetAsAvailable();

        return enemy;
    }

    public static void SetGravity(float gravity)
    {
        foreach (var enemy in Current)
        {
            enemy.SetGravity(gravity);
        }
    }
}