using UnityEngine;

public class GravitySwitcher : MonoBehaviour
{
    [SerializeField] int Gravity = 1;

    private bool _isProcessing = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isProcessing)
            return;

        _isProcessing = true;

        if (collision.GetComponent<Player>().IsGrounded)
            GameManager.SetGravity(Gravity);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isProcessing = false;
    }
}
