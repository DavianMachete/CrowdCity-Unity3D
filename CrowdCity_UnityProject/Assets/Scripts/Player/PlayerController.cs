using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public CharacterController characterController;

    void Awake()
    {
        instance = this;
    }

    public void SetPlayer(CharacterController character)
    {
        characterController = character;
    }
}
