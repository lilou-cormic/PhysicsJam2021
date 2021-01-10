using PurpleCable;
using UnityEngine;

public class HealthPickup : Item
{
    protected override void OnPickup(Collider2D collision)
    {
        collision.GetComponent<Health>().ChangeHP(1);
    }
}
