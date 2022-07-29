using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int currentLevelNumber = 0;

    void Awake()
    {
        instance = this;
    }

    public void InitLevel()
    {
        GetLevelNumber();

        GenerateLevel();
    }

    void GetLevelNumber()
    {
        currentLevelNumber = DataManager.instance.GetLevelNumber();

        GameManager.instance.menuView.UpdateLevelNumber(currentLevelNumber + 1);
        GameManager.instance.levelEndView.UpdateLevelNumber(currentLevelNumber + 1);
    }


    void GenerateLevel()
    {
        //Level Generation Code Here
    }


    public void LevelCompleted()
    {
        DataManager.instance.IncreaseLevelNumber();
    }

    public void LevelFailed()
    {

    }
}
