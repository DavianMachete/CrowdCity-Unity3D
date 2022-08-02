using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Building building;

    public WallSides wallSide;
    public Vector3 Normal { get { return transform.up; } }

    public void OnCharacterInteractedWithWallSide(WallSide wall,Collider collider)
    {
        building.OnCharacterInteractedWithWall(wall, collider);
    }
}
