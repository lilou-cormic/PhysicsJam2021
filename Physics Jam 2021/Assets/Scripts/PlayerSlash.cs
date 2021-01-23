using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            enemy.Kill();

            if (!GameManager.Player.HasFullHealth)
            {
                if (Random.Range(0, 100) < (GameManager.Player.HasOneHPLeft ? 50 : 25))
                {
                    var healthPickup = HealthPickupPool.Current.GetItem();
                    healthPickup.transform.position = enemy.transform.position;
                }
            }
        }
    }
}
