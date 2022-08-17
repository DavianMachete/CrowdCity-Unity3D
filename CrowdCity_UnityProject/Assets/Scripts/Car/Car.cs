using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Renderer carcass;

    private Material baseCarcassMaterial;


    public void SetCarcassMaterial(Material material)
    {
        carcass.sharedMaterial = material;
    }

    public Material GetCarcassMaterial()
    {
        if (baseCarcassMaterial == null)
            baseCarcassMaterial = carcass.sharedMaterial;

        return baseCarcassMaterial;
    }
}
