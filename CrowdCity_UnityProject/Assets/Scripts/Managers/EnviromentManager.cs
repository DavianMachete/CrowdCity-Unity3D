using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnviromentManager : MonoBehaviour
{
    public static EnviromentManager instance;

    public Vector2 areaSize = new Vector2(250f, 240f);

    void Awake()
    {
        instance = this;
    }

    public void InitEnviroment()
    {
        //Enviroment Generation Code Here
    }

    public Vector3 GetRandomPosInArea()
    {
        float randomX = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float randomY = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);

        return new Vector3(randomX, 0f, randomY);
    }
}
