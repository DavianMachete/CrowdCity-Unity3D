using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransparentMaker : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private LayerMask layerMask;
    [Space(30)]
    [SerializeField] private UnityEvent onObjectBehindOf;
    [SerializeField] private UnityEvent onObjectNotBehindOf;


    private Material baseMaterialHolder;

    private void OnEnable()
    {
        if (baseMaterialHolder == null)
        {
            baseMaterialHolder = objectRenderer.material;
        }
    }

    public virtual void OnObjectBehindOf(Collider collider)
    {
        if (collider.gameObject.layer== (int)Mathf.Log(layerMask.value, 2))
        {
            objectRenderer.material = transparentMaterial;
            onObjectBehindOf?.Invoke();
        }

    }

    public virtual void OnObjectNotBehindOf(Collider collider)
    {
        if (collider.gameObject.layer == (int)Mathf.Log(layerMask.value, 2))
        {
            objectRenderer.material = baseMaterialHolder;
            onObjectNotBehindOf?.Invoke();
        }
    }
}
