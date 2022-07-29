using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBehaviour : MonoBehaviour
{
    [SerializeField] private List<Color> colors;

    public void SetMaterial(int colorIndex)
    {
        if(colorIndex >= 0 && colorIndex < colors.Count)
        {
            this.GetComponent<MeshRenderer>().material.color = colors[colorIndex];
        }
    }
}
