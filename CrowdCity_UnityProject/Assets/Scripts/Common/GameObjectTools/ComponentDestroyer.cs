using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComponentDestroyer : MonoBehaviour
{
    public Transform myObject;
    private Collider[] childCollider;

    public void DestroyColliders()
    {
        foreach (Transform child in myObject)
        {
            childCollider = child.GetComponentsInChildren<Collider>();

            for (int i = 0; i < childCollider.Length; i++)
            {
                DestroyImmediate(childCollider[i]);
            }
        }
    }
}

