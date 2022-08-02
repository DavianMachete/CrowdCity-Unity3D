using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovementController : MonoBehaviour
{
    public MovementType movementType;
    public Vector3 Velocity = Vector3.zero;
    public Wall currentWall { get; private set; }


    private NavMeshAgent navMeshAgent;
    private CharacterController character;
    private Collider characterCollider;

    private JoystickWalking joystickWalk = null;
    private RandomWalking randomWalk = null;
    private FollowLeaderWalking followLeaderWolking = null;


    [SerializeField] private float wallOffset = 0.5f;


    public void Prepare(CharacterController character)
    {
        if(navMeshAgent==null)
            navMeshAgent = GetComponent<NavMeshAgent>();
        if (characterCollider == null)
            characterCollider = GetComponent<Collider>();

        StopUpdateMovement();
        //navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        //navMeshAgent.ResetPath();

        this.character = character;

        SetSpeed();
        SetPriority();
        movementType = MovementType.AI;
        if (character.Roll == CharacterRoll.Leader)
        {
            if (character.Clan == Clan.Player)
            {
                movementType = MovementType.Joystick;
            }
        }
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return navMeshAgent;
    }

    public void StartUpdateMovement()
    {
        update = true;
        if (IUpdateMovementHelper == null)
            IUpdateMovementHelper = StartCoroutine(IUpdateMovement());
    }

    public void StopUpdateMovement()
    {
        update = false;
        if (IUpdateMovementHelper != null)
            StopCoroutine(IUpdateMovementHelper);
        IUpdateMovementHelper = null;
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
        if(character.Clan==Clan.Player)
        {
            navMeshAgent.speed = character.Roll == CharacterRoll.Leader ?
                CharacterManager.instance.playerSpeed : CharacterManager.instance.followersSpeed;
            return;
        }

        if (character.Clan == Clan.None)
        {
            navMeshAgent.speed = CharacterManager.instance.freeCharactersSpeed;
            return;
        }

        if (character.Clan != Clan.Player)
        {
            navMeshAgent.speed = character.Roll == CharacterRoll.Leader?
                CharacterManager.instance.oponentsSpeed: CharacterManager.instance.followersSpeed;
            return;
        }
    }

    private void SetPriority()
    {
        if (character.Roll == CharacterRoll.Leader)
            navMeshAgent.avoidancePriority = 0;
        else
            navMeshAgent.avoidancePriority = 2;

    }

    private void MoveOnWall(WallSide wallSide)
    {
        if (IMoveToWallHelper == null)
            IMoveToWallHelper = StartCoroutine(IMoveToWall(wallSide));
    }

    #endregion

    #region Coroutines

    private bool isMoveToWall = false;
    private Coroutine IMoveToWallHelper;
    private IEnumerator IMoveToWall(WallSide wallSide)
    {
        //Turn Off other movement and iteraction controllers

        isMoveToWall = true;
        characterCollider.enabled = false;
        navMeshAgent.ResetPath();
        navMeshAgent.enabled = false;

        //


        Vector3 startPos = transform.position;

        Vector3 wallSidePoint = Utilities.FindNearestPointOnLine(wallSide.LCorner, wallSide.RCorner, startPos);

        Vector3 targetPos = Vector3.zero;

        if (wallSide.nextWallSide == null)
        {
            if (currentWall != null)//so target pos must be on ground
            {
                targetPos = wallSide.wall.Normal * wallOffset + wallSidePoint;
                currentWall = null;
            }
            else//so target pos must be on wall
            {
                targetPos = wallSide.wall.transform.forward * wallOffset + wallSidePoint;
                currentWall = wallSide.wall;
            }
        }
        else
        {
            targetPos = wallSide.wall.Normal * -1f + wallSidePoint;
            currentWall = wallSide.nextWallSide.wall;
        }

        float lenght = Vector3.Distance(startPos, wallSidePoint) + Vector3.Distance(wallSidePoint, targetPos);
        float dur = lenght / navMeshAgent.speed;

        float t = 0f;


        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation((wallSidePoint - startPos).normalized, transform.up);

        //first side
        while (t * 2f <= dur)
        {
            //Move
            transform.position = Vector3.Lerp(startPos, wallSidePoint, t * 2f / dur);

            //Rotate
            transform.rotation = Quaternion.Lerp(startRot, targetRot, t * 2f / dur);

            t += Time.deltaTime;
            yield return null;
        }

        //second side
        t = 0f;
        startRot = transform.rotation;
        targetRot = Quaternion.LookRotation((targetPos - wallSidePoint).normalized,
            currentWall != null ? currentWall.Normal : Vector3.up);
        while (t * 2f <= dur)
        {
            //Move
            transform.position = Vector3.Lerp(wallSidePoint, targetPos, t * 2f / dur);

            //Rotate
            transform.rotation = Quaternion.Lerp(startRot, targetRot, t * 2f / dur);

            t += Time.deltaTime;
            yield return null;
        }

        characterCollider.enabled = true;
        navMeshAgent.enabled = true;

        isMoveToWall = false;
        IMoveToWallHelper = null;
    }

    private bool update = false;
    private Coroutine IUpdateMovementHelper;
    private IEnumerator IUpdateMovement()
    {
        while (update)
        {
            if (character.Roll == CharacterRoll.Leader && character.Clan!= Clan.None)
            {
                if (movementType == MovementType.Joystick)
                {
                    if (joystickWalk == null)
                        joystickWalk = new JoystickWalking(this);
                    if (!isMoveToWall)
                        Velocity=joystickWalk.UpdateJoystickWalk();
                }
                else
                {
                    if (randomWalk == null)
                        randomWalk = new RandomWalking(navMeshAgent);
                    if (!isMoveToWall)
                        Velocity = randomWalk.UpdateRandomWalk();
                }
            }
            else if(character.Roll == CharacterRoll.Follower && character.Clan != Clan.None)
            {
                if (followLeaderWolking == null)
                    followLeaderWolking = new FollowLeaderWalking(character);

                if (character.leader == null)
                    character.leader = CrowdManager.instance.GetLeader(character.Clan);
                if (!isMoveToWall)
                    Velocity = followLeaderWolking.UpdateFollowLeaderWalking(character.leader,CharacterManager.instance.followLeaderType);
            }
            else if(character.Clan == Clan.None)
            {
                if (randomWalk == null)
                    randomWalk = new RandomWalking(navMeshAgent);
                if (!isMoveToWall)
                    Velocity = randomWalk.UpdateRandomWalk();
            }

            character.animationController.SetCharacterSpeed(isMoveToWall ? 1f : GetSpeedT());

            yield return null;
        }
    }

    #endregion
}
