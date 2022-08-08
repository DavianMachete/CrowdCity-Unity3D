using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSide : MonoBehaviour
{
    public Wall wall;
    public WallSide nextWallSide;

    public Sides side;

    public Vector3 RCorner { get { return transform.position + transform.right * GetLenghtBySide() / 2f; } }
    public Vector3 LCorner { get { return transform.position - transform.right * GetLenghtBySide() / 2f; } }

    private BoxCollider sideCollider;

    private void Awake()
    {
        sideCollider = GetComponent<BoxCollider>();

        float lenght = GetLenghtBySide();//x
        float height = GetHeight();//y
        float width = GetWidth();//z

        if (wall.BuildingSide == BuildingSide.Ground)
        {
            sideCollider.center =
                new Vector3(0f,
                0.125f / height,
                -0.33f / width);

            sideCollider.size =
                new Vector3((lenght - 0.3f) / lenght,
                0.25f / height,
                0.15f / width);
        }
        else if (wall.BuildingSide == BuildingSide.Top)
        {
            sideCollider.center =
                new Vector3(0f,
                0.125f / height,
                -0.075f / width);

            sideCollider.size =
                   new Vector3((lenght - 0.3f) / lenght,
                   0.25f / height,
                   0.15f / width);
        }
        else
        {
            if (name.Contains("Top"))
            {
                sideCollider.center =
                    new Vector3(0f,
                    0.125f / height,
                    -0.075f / width);

                sideCollider.size =
                       new Vector3((lenght - 0.3f) / lenght,
                       0.25f / height,
                       0.15f / width);
            }
            else if (name.Contains("Bottom"))
            {
                sideCollider.center =
                    new Vector3(0f,
                    0.125f / height,
                    -0.33f / width);

                sideCollider.size =
                       new Vector3((lenght - 0.3f) / lenght,
                       0.25f / height,
                       0.15f / width);
            }
            else if (name.Contains("Right"))
            {
                sideCollider.center =
                    new Vector3(-0.125f / lenght,
                    0.125f / height,
                    -0.075f / width);

                sideCollider.size =
                       new Vector3((lenght - 0.55f) / lenght,
                       0.25f / height,
                       0.15f / width);
            }
            else if (name.Contains("Left"))
            {
                sideCollider.center =
                    new Vector3(0.125f / lenght,
                    0.125f / height,
                    -0.075f / width);

                sideCollider.size =
                       new Vector3((lenght - 0.55f) / lenght,
                       0.25f / height,
                       0.15f / width);
            }
        }
    }

    public void OnCharacterInteracted(Collider collider)
    {
        wall.OnCharacterInteractedWithWallSide(this, collider);
    }

    private float GetLenghtBySide()
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

    private float GetWidth()
    {
        return Vector3.Distance(wall.transform.position, transform.position) * 2f;
    }

    private float GetHeight()
    {
        return Vector3.Distance(nextWallSide.transform.position, nextWallSide.wall.transform.position) * 2f;
    }
}
