using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private Building building;

    public bool isBottom;

    public Sides side;

    public Building Building { get { return building; } }

    public Vector3 LBCorner { get { return transform.position - transform.right * GetWidthBySide() / 2f; } }
    public Vector3 RBCorner { get { return transform.position + transform.right * GetWidthBySide() / 2f; } }

    public Vector3 LTCorner { get { return transform.position + Vector3.up * building.Height - transform.right * GetWidthBySide() / 2f; } }
    public Vector3 RTCorner { get { return transform.position + Vector3.up * building.Height + transform.right * GetWidthBySide() / 2f; } }


    public void OnCharacterInteracted(Collider collider)
    {
        Building.OnCharacterInteractedWithWall(this, collider);
    }


    public float GetWidthBySide()
    {
        switch (side)
        {
            case Sides.Front:
            case Sides.Back:
                return building.ScaleX;
            case Sides.Right:
            case Sides.Left:
                return building.ScaleZ;
            default:
                return 0f;
        }
    }
}
