using System.Collections;
using System.Collections.Generic;
using Machete.Character;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Character characterController;

    void Awake()
    {
        instance = this;
    }

    public void SetPlayer(Character character)
    {
        characterController = character;
    }
}
