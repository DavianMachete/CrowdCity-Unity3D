using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    public float playerSpeed = 8f;
    public float oponentsSpeed = 7.8f;
    public float followersSpeed = 9f;
    public float freeCharactersSpeed = 4f;

    public FollowLeaderType followLeaderType;
    public FollowerPositioning followerPositioning;

    public float radius = 1f;

    public bool isVelocityOfPlayerStatic = false;
    public bool stopWhenJoystickDisable = false;

    public float GreatestSpeed
    {
        get
        {
            float maxS = playerSpeed;
            if (maxS < oponentsSpeed)
                maxS = oponentsSpeed;
            if (maxS < followersSpeed)
                maxS = oponentsSpeed;
            if (maxS < followersSpeed)
                maxS = followersSpeed;
            if (maxS < freeCharactersSpeed)
                maxS = freeCharactersSpeed;
            return maxS;
        }
    }

    public List<Vector3> positionVectors;

    private readonly float PI = 3.14159265359f;

    private void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        positionVectors = GeneratePositionVectors();
    }

    public void SetPlayerSpeed(float s)
    {
        playerSpeed = s;
    }

    public void SetOponentsSpeed(float s)
    {
        oponentsSpeed = s;
    }

    public void SetFollowersSpeed(float s)
    {
        followersSpeed = s;
    }

    public void SetFreeCharactersSpeed(float s)
    {
        freeCharactersSpeed = s;
    }

    public void SetStaticPlayerVelocity(bool value)
    {
        isVelocityOfPlayerStatic = value;
    }

    public void SetStopPlayerOnJoystickDisable(bool value)
    {
        stopWhenJoystickDisable = value;
    }

    public void SetCharactersMinimumDistance(float distance)
    {
        radius = distance;
    }

    private List<Vector3> GeneratePositionVectors()
    {
        List<Vector3> vectors = new List<Vector3>();
        Vector3 pos;


        float currentCircleRadius = radius;
        while (vectors.Count < CrowdManager.instance.CharactersMaxLimit)
        {
            float currentCircleLength = 2 * Mathf.PI * currentCircleRadius;

            float posAmountInCurrentCircle = currentCircleLength / radius;
            float deltaAngle = 360f / posAmountInCurrentCircle;

            float currentAngle = 0;
            for (int i = 0; i < posAmountInCurrentCircle; i++)
            {
                pos = Quaternion.AngleAxis(currentAngle, Vector3.up) * Vector3.right;
                pos *= currentCircleRadius;

                vectors.Add(pos);

                currentAngle += deltaAngle;
            }

            currentCircleRadius += radius;
        }


        return vectors;






        //List <Vector3> vectors = new List<Vector3>();

        //float c = 2f * Mathf.PI * radius;
        //int counter = 1;
        //int wave = 1;
        //for (int i = 1; i <= CrowdManager.instance.CharactersMaxLimit; i++)
        //{
        //    Vector3 pos = Vector3.forward * radius;
        //    float cr = c / radius;
        //    if (cr + counter < i)
        //    {
        //        wave++;
        //        c *= 2;
        //        counter += Mathf.FloorToInt(cr);
        //    }

        //    float angle = 360f / cr * (i - counter);
        //    pos = Quaternion.AngleAxis(angle, Vector3.up) * pos;
        //    pos *= wave * radius;
        //    vectors.Add(pos);
        //}
        //return vectors;
    }
}
