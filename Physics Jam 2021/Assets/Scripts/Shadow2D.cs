using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Shadow2D : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    [SerializeReference] SpriteRenderer SpriteRendererToShadow = null;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        if (SpriteRendererToShadow != null)
        {
            SpriteRenderer.sprite = SpriteRendererToShadow.sprite;

            transform.position = new Vector3(SpriteRendererToShadow.transform.position.x + 0.2f, SpriteRendererToShadow.transform.position.y - 0.05f, 0);

            transform.localScale = SpriteRendererToShadow.transform.localScale;
        }
    }
#endif

    private void Update()
    {
        SpriteRenderer.sprite = SpriteRendererToShadow.sprite;

        transform.position = new Vector3(SpriteRendererToShadow.transform.position.x + 0.2f, SpriteRendererToShadow.transform.position.y - 0.05f, 0);

        transform.localScale = SpriteRendererToShadow.transform.localScale;
    }
}
