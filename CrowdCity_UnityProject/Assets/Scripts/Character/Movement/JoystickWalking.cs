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
        targetDirection.y = 0f;

        if (targetDirection.magnitude < 0.01f)
        {
            targetDirection = prevTargetDirection;
        }
        else
        {
            prevTargetDirection = targetDirection;
        }

        Vector3 currentDir = Utilities.GetDirectionByWall(movementController.CurrentWall,targetDirection).normalized;

        m_Agent.Move(m_Agent.speed * Time.deltaTime * currentDir);

        if (targetDirection!= Vector3.zero)
        { 
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentDir.normalized,
                movementController.CurrentWall==null? Vector3.up: movementController.CurrentWall.Normal),
                Time.deltaTime * 8f);
        }

        return m_Agent.speed * targetDirection.normalized;
    }
}
