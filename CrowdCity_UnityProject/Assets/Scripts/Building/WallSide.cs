using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSide : MonoBehaviour
{
    public Wall wall;
    public WallSide nextWallSide;

    public Sides side;

    public Vector3 RCorner { get { return transform.position + transform.right * GetWidthBySide() / 2f; } }
    public Vector3 LCorner { get { return transform.position - transform.right * GetWidthBySide() / 2f; } }

    public void OnCharacterInteracted(Collider collider)
    {
        wall.OnCharacterInteractedWithWallSide(this, collider);
    }

    public float GetWidthBySide()
    {
        switch (side)
        {
            case Sides.X:
                return wall.building.ScaleX;
            case Sides.Z:
                return wall.building.ScaleZ;
            case Sides.Y:
                return wall.building.Height;
            default:
                return 0f;
        }
    }
}
