using PurpleCable;
using UnityEngine;

public class HealthPickupPool : Pool<HealthPickup>
{
    public static HealthPickupPool Current { get; private set; } = null;

    [SerializeField] HealthPickup Prefab = null;

    protected override void Awake()
    {
        base.Awake();

        Current = this;
    }

    protected override HealthPickup CreateItem()
    {
        HealthPickup healthPickup = Instantiate(Prefab, transform);
        ((IPoolable)healthPickup).SetAsAvailable();

        return healthPickup;
    }

    public static void SetGravity(float gravity)
    {
        foreach (var healthPickup in Current)
        {
            healthPickup.SetGravity(gravity);
        }
    }
}
