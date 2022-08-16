using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Machete.Character
{
    public class FollowLeaderWalking
    {
        private readonly Character controller;

        private CharacterMovementController movementController;
        private CharacterController characterController;

        public FollowLeaderWalking(Character controller)
        {
            this.controller = controller;

            movementController = controller.movementController;
            characterController = movementController.CharacterController;
        }



        public Vector3 UpdateFollowLeaderWalking(Character leaderCC)
        {
            if (leaderCC.movementController.GetSpeedT() < 0.1f)
                return Vector3.zero;

            Vector3 dir = GetDirectionToLocaltionAround(leaderCC);
            dir.y = 0f;

            dir += leaderCC.movementController.Velocity;
            dir = Vector3.ClampMagnitude(dir, movementController.Speed);

            Vector3 currentDir = Utilities.GetDirectionByWall(controller.movementController.CurrentWall, dir);

            characterController.SimpleMove(10f * Time.deltaTime * currentDir);

            if (currentDir != Vector3.zero)
            {
                movementController.transform.rotation = Quaternion.Lerp(movementController.transform.rotation, Quaternion.LookRotation(currentDir.normalized,
                    controller.movementController.CurrentWall == null ? Vector3.up : controller.movementController.CurrentWall.Normal),
                    Time.deltaTime * 8f);
            }

            return currentDir;
        }

        private Vector3 GetDirectionToLocaltionAround(Character leaderCC)
        {
            return GetLocationAround(leaderCC) - controller.transform.position;
        }

        private Vector3 GetLocationAround(Character leaderCC, bool withAngle = true)
        {
            float angle = 0f;
            if (withAngle)
                angle = Vector3.SignedAngle(Vector3.forward, leaderCC.transform.forward, Vector3.up);
            Vector3 pos =
                leaderCC.transform.position +
                //Quaternion.AngleAxis(angle, Vector3.up) *
                CharacterManager.instance.positionVectors[controller.indexInCrowd - 1];
            return pos;
        }

        private Vector3 GetRandomLocationAround(Character leaderCC)
        {
            //float range = controller.indexInCrowd * CharacterManager.instance.radius / 2f;
            Vector3 randomDirection = Random.insideUnitSphere * 5f;//range;

            randomDirection += leaderCC.transform.position + leaderCC.transform.forward * 2f;

            return randomDirection;
        }

        //private Vector3 GetLocationAroundOnNavMesh(CharacterController leaderCC)
        //{
        //    switch (CharacterManager.instance.followerPositioning)
        //    {
        //        case FollowerPositioning.ByRoundWaves:
        //            return Utilities.GetLocation(GetLocationAround(leaderCC));
        //        case FollowerPositioning.ByRoundWavesNoChangeAngle:
        //            return Utilities.GetLocation(GetLocationAround(leaderCC, false));
        //        case FollowerPositioning.RandomAround:
        //            return Utilities.GetLocation(GetRandomLocationAround(leaderCC));
        //        default:
        //            return Utilities.GetLocation(GetRandomLocationAround(leaderCC));
        //    }
        //}
    }
}
