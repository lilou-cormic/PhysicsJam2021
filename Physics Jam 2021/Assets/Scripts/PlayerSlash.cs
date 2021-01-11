using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponentInParent<Enemy>()?.Kill(true);
    }
}
