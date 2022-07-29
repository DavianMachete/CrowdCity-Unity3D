using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    void Awake()
    {
        instance = this;
    }

    public int GetLevelNumber()
    {
       int currentLevelNumber = 0;

        if (!PlayerPrefs.HasKey("levelNumber"))
        {
            PlayerPrefs.SetInt("levelNumber", currentLevelNumber);
        }

        currentLevelNumber = PlayerPrefs.GetInt("levelNumber", currentLevelNumber);

        return currentLevelNumber;

    }

    [ContextMenu("Increase Level Number")]
    public void IncreaseLevelNumber()
    {
        PlayerPrefs.SetInt("levelNumber", GetLevelNumber() + 1);
    }

    public void SetBestScore(int newBest)
    {
        PlayerPrefs.SetInt("bestScore", newBest);
    }
    public int GetBestScore()
    {
        int bestScore = 0;
        if (!PlayerPrefs.HasKey("bestScore"))
        {
            PlayerPrefs.SetInt("bestScore", bestScore);
        }

        bestScore = PlayerPrefs.GetInt("bestScore", bestScore);

        return bestScore;
    }


    public void SetCoinsAmount(int newTotalCoins)
    {
        PlayerPrefs.SetInt("coinsAmount", newTotalCoins);
    }
    public int GetCoinsAmount()
    {
        int coinsAmount = 0;
        if (!PlayerPrefs.HasKey("coinsAmount"))
        {
            PlayerPrefs.SetInt("coinsAmount", coinsAmount);
        }

        coinsAmount = PlayerPrefs.GetInt("coinsAmount", coinsAmount);

        return coinsAmount;
    }

   
    [ContextMenu("Reset Level Number")]
    public void ResetLevelNumber()
    {
        PlayerPrefs.SetInt("levelNumber", 0);
    }
}
