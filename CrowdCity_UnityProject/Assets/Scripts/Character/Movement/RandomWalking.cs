using UnityEngine;
using UnityEngine.AI;

namespace Machete.Character
{
    // Walk to a random position and repeat
    public class RandomWalking
    {
        private CharacterMovementController movementController;
        private readonly CharacterController characterController;
        private Vector3 targetPosition;

        private Vector3 diff;
        private Vector3 velocity;

        public RandomWalking(CharacterMovementController movementController)
        {
            this.movementController = movementController;

            characterController = movementController.CharacterController;
            targetPosition = movementController.transform.position;
        }

        public Vector3 UpdateRandomWalk()
        {
            if (Vector3.Distance(targetPosition, movementController.transform.position) < 0.2f)
                targetPosition = Utilities.GetRandomLocation();

            diff = targetPosition - movementController.transform.position;

            velocity = 10f * movementController.Speed * Time.deltaTime * diff.normalized;

            characterController.SimpleMove(velocity);

            return velocity;
        }
    }
}