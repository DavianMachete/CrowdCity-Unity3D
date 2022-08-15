using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Machete.Character
{
    public class CharacterMovementController : MonoBehaviour
    {
        public MovementType movementType;
        public Vector3 Velocity = Vector3.zero;

        public Wall CurrentWall { get; private set; }
        public Character Character { get; private set; }
        public bool IsMoveToWall { get; private set; }
        public float Speed { get; private set; }
        public CharacterController CharacterController { get; private set; }

        private Collider characterCollider;

        private JoystickWalking joystickWalk = null;
        private RandomWalking randomWalk = null;
        private FollowLeaderWalking followLeaderWolking = null;


        [SerializeField] private float wallOffset = 0.5f;
        [SerializeField] private float wallChangeSpeedMultiplier = 2f;

        private void Update()
        {
            if (!update)
                return;
            UpdateMovement();
        }

        public void Prepare(Character character)
        {
            if (CharacterController == null)
                CharacterController = GetComponent<CharacterController>();
            if (characterCollider == null)
                characterCollider = GetComponent<Collider>();

            StopUpdateMovement();

            this.Character = character;

            SetSpeed();

            movementType = MovementType.AI;
            if (character.Clan == Clan.Player)
            {
                if (character.Roll == CharacterRoll.Leader)
                {
                    movementType = MovementType.Joystick;
                }
            }
        }

        public void StartUpdateMovement()
        {
            update = true;
            //if (IUpdateMovementHelper == null)
            //    IUpdateMovementHelper = StartCoroutine(IUpdateMovement());
        }

        public void StopUpdateMovement()
        {
            update = false;
            //if (IUpdateMovementHelper != null)
            //    StopCoroutine(IUpdateMovementHelper);
            //IUpdateMovementHelper = null;
        }

        public void OnInteractWithWall(WallSide wallSide)
        {

            MoveOnWall(wallSide);
        }

        public float GetSpeedT()
        {
            return Mathf.InverseLerp(0f, CharacterManager.instance.GreatestSpeed, Velocity.magnitude);
        }

        #region Perivate Methods

        private void SetSpeed()
        {
            if (Character.Clan == Clan.Player)
            {
                Speed = Character.Roll == CharacterRoll.Leader ?
                    CharacterManager.instance.playerSpeed : CharacterManager.instance.followersSpeed;
                return;
            }

            if (Character.Clan == Clan.None)
            {
                Speed = CharacterManager.instance.freeCharactersSpeed;
                return;
            }

            if (Character.Clan != Clan.Player)
            {
                Speed = Character.Roll == CharacterRoll.Leader ?
                    CharacterManager.instance.oponentsSpeed : CharacterManager.instance.followersSpeed;
                return;
            }
        }

        private void MoveOnWall(WallSide wallSide)
        {
            if (Vector3.Dot(transform.forward, wallSide.transform.forward) < 0)
                return;

            if (IMoveToWallHelper == null)
                IMoveToWallHelper = StartCoroutine(IMoveToWall(wallSide));
        }

        private void UpdateMovement()
        {
            if (Character.Roll == CharacterRoll.Leader && Character.Clan != Clan.None)
            {
                if (movementType == MovementType.Joystick)
                {
                    if (joystickWalk == null)
                        joystickWalk = new JoystickWalking(this);
                    if (!IsMoveToWall)
                        Velocity = joystickWalk.UpdateJoystickWalk();
                }
                else
                {
                    if (randomWalk == null)
                        randomWalk = new RandomWalking(this);
                    if (!IsMoveToWall)
                        Velocity = randomWalk.UpdateRandomWalk();
                }
            }
            else if (Character.Roll == CharacterRoll.Follower && Character.Clan != Clan.None)
            {
                if (followLeaderWolking == null)
                    followLeaderWolking = new FollowLeaderWalking(Character);

                if (Character.Leader == null)
                    Character.Leader = CrowdManager.instance.GetLeader(Character.Clan);
                if (!IsMoveToWall)
                    Velocity = followLeaderWolking.UpdateFollowLeaderWalking(Character.Leader);
            }
            else if (Character.Clan == Clan.None)
            {
                if (randomWalk == null)
                    randomWalk = new RandomWalking(this);
                if (!IsMoveToWall)
                    Velocity = randomWalk.UpdateRandomWalk();
            }

            Character.animationController.SetCharacterSpeed(IsMoveToWall ? 1f : GetSpeedT());
        }

        #endregion

        #region Coroutines

        private Coroutine IMoveToWallHelper;
        private IEnumerator IMoveToWall(WallSide wallSide)
        {
            //Turn Off other movement and iteraction controllers

            IsMoveToWall = true;
            characterCollider.enabled = false;


            Vector3 startPos = transform.position;

            Vector3 wallSidePoint = Utilities.FindNearestPointOnLine(wallSide.LCorner, wallSide.RCorner, startPos);

            Vector3 targetPos = (-1f * wallOffset * wallSide.nextWallSide.transform.forward) + wallSidePoint;


            float lenght = Vector3.Distance(startPos, wallSidePoint);
            float dur = lenght / (Speed * wallChangeSpeedMultiplier);

            float t = 0f;


            //first side
            Quaternion startRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(wallSide.nextWallSide.transform.forward * -1f, wallSide.nextWallSide.wall.Normal);

            while (t <= dur)
            {
                //Move
                transform.position = Vector3.Lerp(startPos, wallSidePoint, t / dur);

                //Rotate
                transform.rotation = Quaternion.Slerp(startRot, targetRot, (t / dur) / 2f);

                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            //second side
            lenght = Vector3.Distance(wallSidePoint, targetPos);
            dur = lenght / (Speed * wallChangeSpeedMultiplier);

            t = 0f;
            while (t <= dur)
            {
                //Move
                transform.position = Vector3.Lerp(wallSidePoint, targetPos, t / dur);

                //Rotate
                transform.rotation = Quaternion.Slerp(startRot, targetRot, 0.5f + (t / dur) / 2f);

                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            transform.rotation = targetRot;

            CurrentWall = wallSide.nextWallSide.wall;

            characterCollider.enabled = true;

            IsMoveToWall = false;

            IMoveToWallHelper = null;
        }

        private bool update = false;
        //private Coroutine IUpdateMovementHelper;
        //private IEnumerator IUpdateMovement()
        //{
        //    while (update)
        //    {
        //        if (Character.Roll == CharacterRoll.Leader && Character.Clan != Clan.None)
        //        {
        //            if (movementType == MovementType.Joystick)
        //            {
        //                if (joystickWalk == null)
        //                    joystickWalk = new JoystickWalking(this);
        //                if (!IsMoveToWall)
        //                    Velocity = joystickWalk.UpdateJoystickWalk();
        //            }
        //            else
        //            {
        //                if (randomWalk == null)
        //                    randomWalk = new RandomWalking(this);
        //                if (!IsMoveToWall)
        //                    Velocity = randomWalk.UpdateRandomWalk();
        //            }
        //        }
        //        else if (Character.Roll == CharacterRoll.Follower && Character.Clan != Clan.None)
        //        {
        //            if (followLeaderWolking == null)
        //                followLeaderWolking = new FollowLeaderWalking(Character);

        //            if (Character.Leader == null)
        //                Character.Leader = CrowdManager.instance.GetLeader(Character.Clan);
        //            if (!IsMoveToWall)
        //                Velocity = followLeaderWolking.UpdateFollowLeaderWalking(Character.Leader);
        //        }
        //        else if (Character.Clan == Clan.None)
        //        {
        //            if (randomWalk == null)
        //                randomWalk = new RandomWalking(this);
        //            if (!IsMoveToWall)
        //                Velocity = randomWalk.UpdateRandomWalk();
        //        }

        //        Character.animationController.SetCharacterSpeed(IsMoveToWall ? 1f : GetSpeedT());

        //        //yield return new WaitForEndOfFrame();
        //        yield return null;
        //    }
        //}

        #endregion
    }
}