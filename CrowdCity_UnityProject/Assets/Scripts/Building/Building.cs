using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Transform buildingTransform;

    public float Height { get { return buildingTransform.localScale.y; } }
    public float ScaleX { get { return buildingTransform.localScale.x; } }
    public float ScaleZ { get { return buildingTransform.localScale.z; } }


    public void OnCharacterInteractedWithWall(WallSide wallSide,Collider collider)
    {
        CharacterMovementController characterMC = collider.GetComponent<CharacterMovementController>();
        characterMC.OnInteractWithWall(wallSide);
    }
}
