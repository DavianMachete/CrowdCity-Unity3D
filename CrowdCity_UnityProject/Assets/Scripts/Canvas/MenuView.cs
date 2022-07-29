using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNumber;

    public void UpdateLevelNumber(int currentLevel)
    {
        levelNumber.text = "Level " + currentLevel;
    }
}
