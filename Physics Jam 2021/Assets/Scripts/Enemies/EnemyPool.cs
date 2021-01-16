using PurpleCable;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class EnemyPool : Pool<Enemy, EnemyType>
{
    public static EnemyPool Current { get; private set; } = null;

    [SerializeField] Enemy[] Prefabs = null;

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
        var prefab = Prefabs.FirstOrDefault(x => x.EnemyType == category) ?? throw new NotImplementedException($"{category} not found in EnemyPool.Prefabs");

        Enemy enemy = Instantiate(prefab, transform);
        enemy.SetSortingOrder(this.Count());
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