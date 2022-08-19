using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnviromentManager : MonoBehaviour
{
    public static EnviromentManager instance;

    public Vector2 areaSize = new Vector2(100f, 100f);

    [SerializeField] private Material buildingWhiteMAterial;


    private Building[] buildings;



    void Awake()
    {
        instance = this;
    }

    public void InitEnviroment()
    {
        //Enviroment Generation Code Here
    }

    public Vector3 GetRandomLocationInArea()
    {
        float randomX = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float randomY = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
        Vector3 randomPos = new Vector3(randomX, 0f, randomY);

        NavMesh.SamplePosition(randomPos, out var hit, 25f, 1);
        Vector3 finalPosition = hit.position;
        return finalPosition;
    }


    public void SetBuildingsMaterial(int index)
    {
        Material material = null;
        switch (index)
        {
            case 0:
                material = null;
                break;
            case 1:
                material = buildingWhiteMAterial;
                break;
            default:
                break;
        }
        Debug.Log("Start Ienumerator");

        if (buildings == null) GetAllBuildings();


        Debug.Log("Building gets");

        for (int i = 0; i < buildings.Length; i++)
        {
            buildings[i].SetMaterial(material);


            Debug.Log($"{buildings[i].name} material changed");
        }
    }

    private void GetAllBuildings()
    {
        buildings = FindObjectsOfType<Building>();
    }


    
}
