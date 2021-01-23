using UnityEngine;

public class BossShield : MonoBehaviour
{
    private void Update()
    {
        transform.localPosition = new Vector3(Mathf.Sin(Time.timeSinceLevelLoad) * 2.85f, Mathf.Cos(Time.timeSinceLevelLoad) * 2.85f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponentInParent<Enemy>();

        if (enemy != null && enemy.HasLeftBoss)
        {
            enemy.Kill();
        }
    }
}