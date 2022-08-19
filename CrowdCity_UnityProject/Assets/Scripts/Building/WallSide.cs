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

    [SerializeField] private bool ignoreX = false;
    [SerializeField] private bool ignoreY = false;
    [SerializeField] private bool ignoreZ = false;

    //private void Awake()
    //{
    //    FixCollider();
    //}

    public void FixCollider()
    {
        sideCollider = GetComponent<BoxCollider>();

        float lenght = GetLenghtBySide();//x
        float height = GetHeight();//y
        float width = GetWidth();//z

        if (wall.BuildingSide == BuildingSide.Ground)
        {
            sideCollider.center =
                new Vector3(ignoreX ? sideCollider.center.x : 0f,
                ignoreY ? sideCollider.center.y : (0.125f / height),
                ignoreZ ? sideCollider.center.z : (-0.33f / width));

            sideCollider.size =
                new Vector3(ignoreX ? sideCollider.size.x : ((lenght - 0.3f) / lenght),
                ignoreY ? sideCollider.size.y : (0.25f / height),
                ignoreZ ? sideCollider.size.z : (0.15f / width));
        }
        else if (wall.BuildingSide == BuildingSide.Top)
        {
            sideCollider.center =
                new Vector3(ignoreX ? sideCollider.center.x : 0f,
                ignoreY ? sideCollider.center.y : (0.125f / height),
                ignoreZ ? sideCollider.center.z : (-0.075f / width));

            sideCollider.size =
                new Vector3(ignoreX ? sideCollider.size.x : ((lenght - 0.3f) / lenght),
                ignoreY ? sideCollider.size.y : (0.25f / height),
                ignoreZ ? sideCollider.size.z : (0.15f / width));
        }
        else
        {
            if (name.Contains("Top"))
            {
                sideCollider.center =
                    new Vector3(ignoreX ? sideCollider.center.x : 0f,
                    ignoreY ? sideCollider.center.y : (0.125f / height),
                    ignoreZ ? sideCollider.center.z : (-0.075f / width));

                sideCollider.size =
                    new Vector3(ignoreX ? sideCollider.size.x : ((lenght - 0.3f) / lenght),
                    ignoreY ? sideCollider.size.y : (0.25f / height),
                    ignoreZ ? sideCollider.size.z : (0.15f / width));
            }
            else if (name.Contains("Bottom"))
            {
                sideCollider.center =
                    new Vector3(ignoreX ? sideCollider.center.x : 0f,
                    ignoreY ? sideCollider.center.y : (0.125f / height),
                    ignoreZ ? sideCollider.center.z : (-0.33f / width));

                sideCollider.size =
                    new Vector3(ignoreX ? sideCollider.size.x : ((lenght - 0.3f) / lenght),
                    ignoreY ? sideCollider.size.y : (0.25f / height),
                    ignoreZ ? sideCollider.size.z : (0.15f / width));
            }
            else if (name.Contains("Right"))
            {
                sideCollider.center =
                    new Vector3(ignoreX ? sideCollider.center.x : (-0.125f / lenght),
                    ignoreY ? sideCollider.center.y : (0.125f / height),
                    ignoreZ ? sideCollider.center.z : (-0.075f / width));

                sideCollider.size =
                       new Vector3(ignoreX ? sideCollider.size.x : ((lenght - 0.55f) / lenght),
                       ignoreY ? sideCollider.size.y : (0.25f / height),
                       ignoreZ ? sideCollider.size.z : (0.15f / width));
            }
            else if (name.Contains("Left"))
            {
                sideCollider.center =
                    new Vector3(ignoreX ? sideCollider.center.x : (-.125f / lenght),
                    ignoreY ? sideCollider.center.y : (0.125f / height),
                    ignoreZ ? sideCollider.center.z : (-0.075f / width));

                sideCollider.size =
                       new Vector3(ignoreX ? sideCollider.size.x : ((lenght - 0.55f) / lenght),
                       ignoreY ? sideCollider.size.y : (0.25f / height),
                       ignoreZ ? sideCollider.size.z : (0.15f / width));
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
