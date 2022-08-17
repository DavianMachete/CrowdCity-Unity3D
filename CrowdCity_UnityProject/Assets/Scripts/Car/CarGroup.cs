using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGroup : MonoBehaviour
{
    public GameObject carPrefab;

    private Transform[] transforms;
    private Material[] materials;

    private GameObject[] cars;

    [SerializeField] private List<Material> materialsForRandom;

    public void ReplaceWithPrefabs()
    {
        int childCount = transform.childCount;
        string name = carPrefab.name;

        cars = new GameObject[childCount];
        transforms = new Transform[childCount];
        materials = new Material[childCount];

        for (int i = 0; i < childCount; i++)
        {
            transforms[i] = transform.GetChild(i);

            Car car = transform.GetChild(i).GetComponent<Car>();
            if (car == null)
            {
                materials[i] = transform.GetChild(i).GetComponent<Renderer>().sharedMaterial;
            }
            else
            {
                materials[i] = transform.GetChild(i).GetComponent<Car>().GetCarcassMaterial();
            }
        }

        for (int i = 0; i < childCount; i++)
        {
            GameObject carGO = Instantiate(carPrefab, transform);
            cars[i] = carGO;

            carGO.name = $"{name} {i + 1}";
            carGO.transform.SetPositionAndRotation(transforms[i].position, transforms[i].rotation);

            Car car = carGO.GetComponent<Car>();
            if (car == null)
                carGO.GetComponent<Renderer>().sharedMaterial = materials[i];
            else
                carGO.GetComponent<Car>().SetCarcassMaterial(materials[i]);
        }

        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transforms[i].gameObject);
        }
    }

    public void RandomizeMaterials()
    {
        cars = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            cars[i] = transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < cars.Length; i++)
        {
            Material m = materialsForRandom[Random.Range(0, materialsForRandom.Count)];

            Car car = cars[i].GetComponent<Car>();
            if (car == null)
                cars[i].GetComponent<Renderer>().sharedMaterial = m;
            else
                cars[i].GetComponent<Car>().SetCarcassMaterial(m);
        }
    }
}
