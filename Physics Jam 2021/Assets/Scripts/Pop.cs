using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Pop : MonoBehaviour, IPoolable
{
    private SpriteRenderer SpriteRenderer = null;

    private Animator Animator = null;

    public Color Color { get => SpriteRenderer.color; set => SpriteRenderer.color = value; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();

        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        Animator.SetTrigger("play");

        yield return new WaitForSeconds(0.4f);

        ((IPoolable)this).SetAsAvailable();
    }

    bool IPoolable.IsInUse
        => gameObject.activeSelf;

    void IPoolable.SetAsInUse()
        => gameObject.SetActive(true);

    void IPoolable.SetAsAvailable()
        => gameObject.SetActive(false);
}