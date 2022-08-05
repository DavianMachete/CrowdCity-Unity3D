using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowLeaderWalking
{
    private readonly CharacterController controller;
    private readonly NavMeshAgent agent;

    public FollowLeaderWalking(CharacterController controller)
    {
        this.controller = controller;

        agent = controller.movementController.GetNavMeshAgent();
    }



    public Vector3 UpdateFollowLeaderWalking(CharacterController leaderCC, FollowLeaderType followLeaderType)
    {
        if(leaderCC.movementController.GetSpeedT() < 0.1f)
            return Vector3.zero;


        if (followLeaderType == FollowLeaderType.ByAgentDestination)
        {
            agent.destination = GetLocationAroundOnNavMesh(leaderCC);
            //agent.SetDestination(GetLocationAroundOnNavMesh(leaderCC));
            return agent.velocity;
        }
        else
        {
            //if (agent.pathPending)
                agent.ResetPath();

            Vector3 dir = GetDirectionToLocaltionAround(leaderCC);
            dir.y = 0f;

            if (followLeaderType == FollowLeaderType.ByAgentMove)
            {
                float speed = 0f; ;
                if (dir.magnitude > 0f)
                {
                    speed = dir.magnitude;

                    agent.Move(speed * Time.deltaTime * dir.normalized);

                    if (dir != Vector3.zero)
                    {
                        agent.transform.rotation =
                            Quaternion.Slerp(agent.transform.rotation,
                            Quaternion.LookRotation(dir.normalized, Vector3.up),
                             Time.deltaTime * 20f);
                    }
                }
                return speed * dir.normalized;
            }
            else
            {
                dir += leaderCC.movementController.Velocity;
                dir = Vector3.ClampMagnitude(dir, agent.speed);

                Vector3 currentDir = Utilities.GetDirectionByWall(controller.movementController.CurrentWall, dir);

                agent.Move(Time.deltaTime * currentDir);

                if (currentDir != Vector3.zero)
                {
                    agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, Quaternion.LookRotation(currentDir.normalized,
                        controller.movementController.CurrentWall == null ? Vector3.up : controller.movementController.CurrentWall.Normal),
                        Time.deltaTime * 8f);
                }

                return currentDir;
            }
        }
    }

    private Vector3 GetDirectionToLocaltionAround(CharacterController leaderCC)
    {
        return GetLocationAround(leaderCC) - agent.transform.position;
    }

    private Vector3 GetLocationAround(CharacterController leaderCC, bool withAngle = true)
    {
        float angle = 0f;
        if(withAngle)
            angle =Vector3.SignedAngle(Vector3.forward, leaderCC.transform.forward, Vector3.up);
        Vector3 pos =
            leaderCC.transform.position +
            Quaternion.AngleAxis(angle, Vector3.up) *
            CharacterManager.instance.positionVectors[controller.indexInCrowd - 1];
        return pos;
    }

    private Vector3 GetRandomLocationAround(CharacterController leaderCC)
    {
        //float range = controller.indexInCrowd * CharacterManager.instance.radius / 2f;
        Vector3 randomDirection = Random.insideUnitSphere * 5f;//range;

        randomDirection += leaderCC.transform.position + leaderCC.transform.forward * 2f;

        return randomDirection;
    }

    private Vector3 GetLocationAroundOnNavMesh(CharacterController leaderCC)
    {
        switch (CharacterManager.instance.followerPositioning)
        {
            case FollowerPositioning.ByRoundWaves:
                return Utilities.GetLocation(GetLocationAround(leaderCC));
            case FollowerPositioning.ByRoundWavesNoChangeAngle:
                return Utilities.GetLocation(GetLocationAround(leaderCC, false));
            case FollowerPositioning.RandomAround:
                return Utilities.GetLocation(GetRandomLocationAround(leaderCC));
            default:
                return Utilities.GetLocation(GetRandomLocationAround(leaderCC));
        }
    }
}
