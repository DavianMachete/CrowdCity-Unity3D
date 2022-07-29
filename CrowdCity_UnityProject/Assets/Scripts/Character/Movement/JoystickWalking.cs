using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class JoystickWalking
{
    private NavMeshAgent m_Agent;
    private Vector3 direction = Vector3.zero;
    private Vector3 prevDirection = Vector3.zero;
    private float t = 0f;
    private Transform transform;

    public JoystickWalking(NavMeshAgent navMeshAgent, Transform transform)
    {
        m_Agent = navMeshAgent;
        this.transform = transform;
    }

    public Vector3 UpdateJoystickWalk()
    {
        direction.x = JoysticManager.instance.Horizontal;
        direction.z = JoysticManager.instance.Vertical;

        t = direction.magnitude;
        direction = direction.normalized;
        if (t == 0f && !CharacterManager.instance.stopWhenJoystickDisable)
            direction = prevDirection;

        if (!CharacterManager.instance.isVelocityOfPlayerStatic&&
            CharacterManager.instance.stopWhenJoystickDisable)
        {
            direction *= t;
        }

        m_Agent.Move(m_Agent.speed * Time.deltaTime * direction);

        if (direction!= Vector3.zero)
        { 
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction.normalized, Vector3.up), Time.deltaTime * 8f);

            //Utilities.GetPointByRayCast(m_Agent.transform.position + m_Agent.transform.forward * 4f);
        }

        if (prevDirection == Vector3.zero)
            prevDirection = transform.forward;

        if (direction != Vector3.zero)
            prevDirection = direction.normalized;

        return m_Agent.speed * direction;
    }
}
