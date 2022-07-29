using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoysticManager : MonoBehaviour
{
    public static JoysticManager instance;
    public Vector3 Velocity { get { return GetJoysticVelocity(); } }
    public float Vertical { get { return floatingJoystick.Vertical; } }
    public float Horizontal { get { return floatingJoystick.Horizontal; } }

    private Vector3 velocity = Vector3.zero;
    private Vector3 prevVelocity = Vector3.zero;

    [SerializeField] private FloatingJoystick floatingJoystick;

    private void Awake()
    {
        instance = this;
        prevVelocity = Vector3.forward;
    }

    private Vector3 GetJoysticVelocity()
    {
        velocity.x = Vertical;
        velocity.z = Horizontal;

        if (velocity.magnitude > 0.1f)
            prevVelocity = velocity;

        if (CharacterManager.instance.isVelocityOfPlayerStatic &&
            !CharacterManager.instance.stopWhenJoystickDisable)
        {
            if (velocity.magnitude > 0.1f)
                return velocity.normalized;
            else
                return prevVelocity.normalized;

        }
        else if (CharacterManager.instance.isVelocityOfPlayerStatic &&
            velocity.magnitude > 0f)
        {
            return velocity.normalized;
        }

        return velocity;
    }
}
