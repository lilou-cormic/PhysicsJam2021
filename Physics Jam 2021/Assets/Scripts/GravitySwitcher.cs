using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GravitySwitcher : MonoBehaviour
{
    private Animator Animator = null;

    [SerializeField] int Gravity = 1;

    private bool _isProcessing = false;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isProcessing)
            return;

        _isProcessing = true;

        if (collision.GetComponent<Player>()?.IsGrounded == true)
        {
            Animator.SetTrigger("switch");

            GameManager.SetGravity(Gravity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isProcessing = false;
    }
}
