using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransparentMaker : MonoBehaviour
{
    [SerializeField] private Transform objectVisualContainer;


    [SerializeField] private Material transparentMaterial;
    [SerializeField] private LayerMask layerMask;
    [Space(30)]
    [SerializeField] private UnityEvent onObjectBehindOf;
    [SerializeField] private UnityEvent onObjectNotBehindOf;


    private Renderer[] renderers;
    private Material[] baseMaterials;

    public virtual void OnObjectBehindOf(Collider collider)
    {
        if (collider.gameObject.layer == (int)Mathf.Log(layerMask.value, 2))
        {
            CheckRenderers();

            SetMaterial(transparentMaterial);

            onObjectBehindOf?.Invoke();
        }

    }

    public virtual void OnObjectNotBehindOf(Collider collider)
    {
        if (collider.gameObject.layer == (int)Mathf.Log(layerMask.value, 2))
        {
            CheckRenderers();

            ChangeMaterialsToBase();

            onObjectNotBehindOf?.Invoke();
        }
    }

    private void SetMaterial(Material material)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sharedMaterial = material;
        }
    }

    private void ChangeMaterialsToBase()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sharedMaterial = baseMaterials[i];
        }
    }

    private void CheckRenderers()
    {
        if (renderers == null)
        {
            renderers = objectVisualContainer.GetComponentsInChildren<Renderer>();


            if (baseMaterials == null)
                baseMaterials = new Material[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                baseMaterials[i] = renderers[i].sharedMaterial;
            }
        }
    }
}
