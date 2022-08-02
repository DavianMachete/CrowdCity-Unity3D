using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class JoystickWalking
{
    private CharacterMovementController movementController;
    private NavMeshAgent m_Agent;
    private Vector3 targetDirection = Vector3.zero;
    private Vector3 prevTargetDirection = Vector3.zero;
    private float t = 0f;
    private Transform transform;

    public JoystickWalking(CharacterMovementController movementController)
    {
        this.movementController = movementController;
        m_Agent = movementController.GetNavMeshAgent();
        transform = movementController.transform;
    }

    public Vector3 UpdateJoystickWalk()
    {
        targetDirection.x = JoysticManager.instance.Horizontal;
        targetDirection.z = JoysticManager.instance.Vertical;

        t = targetDirection.magnitude;
        targetDirection = targetDirection.normalized;
        if (t == 0f && !CharacterManager.instance.stopWhenJoystickDisable)
            targetDirection = prevTargetDirection;

        if (!CharacterManager.instance.isVelocityOfPlayerStatic&&
            CharacterManager.instance.stopWhenJoystickDisable)
        {
            targetDirection *= t;
        }

        targetDirection = GetDirectionByWall();
        m_Agent.Move(m_Agent.speed * Time.deltaTime * targetDirection);

        if (targetDirection!= Vector3.zero)
        { 
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection.normalized, Vector3.up), Time.deltaTime * 8f);

            //Utilities.GetPointByRayCast(m_Agent.transform.position + m_Agent.transform.forward * 4f);
        }

        if (prevTargetDirection == Vector3.zero)
            prevTargetDirection = transform.forward;

        if (targetDirection != Vector3.zero)
            prevTargetDirection = targetDirection.normalized;

        return m_Agent.speed * targetDirection;
    }

    Vector3 newDir = Vector3.zero;
    private Vector3 GetDirectionByWall()
    {
        if (movementController.currentWall == null)
            return targetDirection;

        newDir = Vector3.zero;
        switch (movementController.currentWall.wallSide)
        {
            case WallSides.Front:
                {
                    newDir.x = targetDirection.x;
                    newDir.y = targetDirection.z;
                }
                return newDir;
            case WallSides.Right:
                {
                    newDir.z = targetDirection.z;
                    newDir.y = targetDirection.x * -1f;
                }
                return newDir;
            case WallSides.Back:
                {
                    newDir.x = targetDirection.x;
                    newDir.y = targetDirection.z * -1f;
                }
                return newDir;
            case WallSides.Left:
                {
                    newDir.z = targetDirection.z;
                    newDir.y = targetDirection.x;
                }
                return newDir;
            case WallSides.Top:
                return targetDirection;
            default:
                return targetDirection;
        }
    }
}
