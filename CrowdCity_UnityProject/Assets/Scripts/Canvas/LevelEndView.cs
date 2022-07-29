using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelEndView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI completedLevelNumber;
    [SerializeField] TextMeshProUGUI failedLevelNumber;

    public void UpdateLevelNumber(int currentLevel)
    {
        completedLevelNumber.text = "Level " + currentLevel + " Completed!";
        failedLevelNumber.text = "Level " + currentLevel + " Failed";
    }
}
