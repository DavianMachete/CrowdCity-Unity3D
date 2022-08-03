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
        prevTargetDirection = transform.forward;
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
        Vector3 currentDir = GetDirectionByWall();
        m_Agent.Move(m_Agent.speed * Time.deltaTime * currentDir);

        if (targetDirection!= Vector3.zero)
        { 
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentDir.normalized, Vector3.up), Time.deltaTime * 8f);
        }

        if (targetDirection != Vector3.zero)
            prevTargetDirection = targetDirection;
        return m_Agent.speed * currentDir.normalized;
    }

    Vector3 newDir = Vector3.zero;
    private Vector3 GetDirectionByWall()
    {
        if (movementController.currentWall == null)
            return targetDirection;

        //newDir = Vector3.zero;
        switch (movementController.currentWall.BuildingSide)
        {
            case BuildingSide.Front:
                {
                    newDir.z = 0f;
                    newDir.x = targetDirection.x;
                    newDir.y = targetDirection.z;
                }
                return newDir;
            case BuildingSide.Right:
                {
                    newDir.x = 0f;
                    newDir.z = targetDirection.z;
                    newDir.y = targetDirection.x * -1f;
                }
                return newDir;
            case BuildingSide.Back:
                {
                    newDir.z = 0f;
                    newDir.x = targetDirection.x;
                    newDir.y = targetDirection.z * -1f;
                }
                return newDir;
            case BuildingSide.Left:
                {
                    newDir.x = 0f;
                    newDir.z = targetDirection.z;
                    newDir.y = targetDirection.x;
                }
                return newDir;
            case BuildingSide.Top:
            case BuildingSide.Ground:
            default:
                {
                    newDir.y = 0f;
                    newDir.z = targetDirection.z;
                    newDir.x = targetDirection.x;
                }
                return newDir;
        }
    }
}
