using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Building buildingContinues;
    [SerializeField] private Transform visualContainer;
    [SerializeField] private Transform buildingTransform;
    [SerializeField] private BoxCollider triggerCollider;

    public float Height { get { return buildingTransform.localScale.y; } }
    public float ScaleX { get { return buildingTransform.localScale.x; } }
    public float ScaleZ { get { return buildingTransform.localScale.z; } }

    [SerializeField] private List<Wall> walls;

    private Renderer[] renderers;
    private Material[] baseMaterials;

    public void OnCharacterInteractedWithWall(WallSide wallSide,Collider collider)
    {
        CharacterMovementController characterMC = collider.GetComponent<CharacterMovementController>();
        characterMC.OnInteractWithWall(wallSide);
    }

    public void FixColliders()
    {
        foreach (Wall wall in walls)
        {
            wall.FixColliders();
        }

        if(triggerCollider != null)
        {
            float h = Height;
            if (buildingContinues != null)
                h += buildingContinues.Height;

            float hs = h / ScaleZ;

            triggerCollider.center =
                new Vector3(0f,
                            0.5f,
                            (hs + (1f/ ScaleZ))/ 2f);

            triggerCollider.size =
                new Vector3((ScaleX + 2f) / ScaleX,
                            1f,
                            hs);
        }
    }

    public void SetMaterial(Material material)
    {
        CheckRenderers();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sharedMaterial = material != null ? material : baseMaterials[i];
        }
    }

    private void CheckRenderers()
    {
        if (renderers == null)
        {
            renderers = visualContainer.GetComponentsInChildren<Renderer>();


            if (baseMaterials == null)
                baseMaterials = new Material[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                baseMaterials[i] = renderers[i].sharedMaterial;
            }
        }
    }
}
