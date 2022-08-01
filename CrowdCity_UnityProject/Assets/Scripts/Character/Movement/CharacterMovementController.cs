using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovementController : MonoBehaviour
{
    public MovementType movementType;
    public Vector3 Velocity = Vector3.zero;


    private NavMeshAgent navMeshAgent;
    private CharacterController character;
    private Collider characterCollider;

    private JoystickWalking joystickWalk = null;
    private RandomWalking randomWalk = null;
    private FollowLeaderWalking followLeaderWolking = null;


    [SerializeField] private float onWallSpeedMultiplier = 1f;
    [SerializeField] private float distanceFromWall = 0.5f;


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

    public void OnInteractWithWall(Collider wallCollider)
    {
        if (onWall)
            return;

        Wall wall = wallCollider.GetComponent<Wall>();
        //Debug.Log($"interacted with wall {wall.gameObject.name}");

        MoveOnWall(wall);
    }

    public void OnInteractWithWall(Collision wallCollision)
    {
        if (onWall )
            return;
        Wall wall = wallCollision.gameObject.GetComponent<Wall>();

        MoveOnWall(wall);
    }

    public void OnInteractWithWall(Wall wall)
    {
        if (onWall)
            return;

        MoveOnWall(wall);
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

    private void MoveOnWall(Wall wall)
    {
        if (movementType != MovementType.Joystick)
            return;
        if (IMoveOnWallHelper == null)
            IMoveOnWallHelper = StartCoroutine(IMoveOnWall(wall));
    }

    #endregion

    #region Coroutines

    private bool onWall = false;
    private Coroutine IMoveOnWallHelper;
    private IEnumerator IMoveOnWall(Wall wall)
    {
        onWall = true;
        characterCollider.enabled = false;
        navMeshAgent.ResetPath();
        navMeshAgent.enabled = false;

        float speed = navMeshAgent.speed * onWallSpeedMultiplier;

        
        float vmDur = wall.Building.Height/ speed;
        float height = wall.isBottom ? wall.Building.Height : wall.Building.Height * -1f;

        float t = 0f;
        Vector3 startPos = transform.position;
        //Vector3 endPos = startPos + dir;
        Vector3 endPos = wall.isBottom ?
            Utilities.FindNearestPointOnLine(wall.LBCorner, wall.RBCorner, startPos) + wall.transform.forward * distanceFromWall :
            Utilities.FindNearestPointOnLine(wall.LTCorner, wall.RTCorner, startPos) + wall.transform.forward * distanceFromWall;
        float hmDur = Vector3.Distance(startPos, endPos) / speed;

        Quaternion stratRot = transform.rotation;
        Vector3 rotateTo = wall.isBottom ? wall.transform.forward * -1f : wall.transform.forward;

        while (t < hmDur)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t / hmDur);
            transform.rotation = Quaternion.Lerp(stratRot, Quaternion.LookRotation(rotateTo, Vector3.up), t/ hmDur);

            t += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;

        t = 0f;
        startPos = transform.position;
        endPos = startPos + (height * Vector3.up);

        while (t < vmDur)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t / vmDur);

            t += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;

        t = 0f;
        startPos = transform.position;
        endPos = wall.isBottom ?
            Utilities.FindNearestPointOnLine(wall.LTCorner, wall.RTCorner, startPos) - (2f * distanceFromWall * wall.transform.forward) :
            Utilities.FindNearestPointOnLine(wall.LBCorner, wall.RBCorner, startPos) + (2f * distanceFromWall * wall.transform.forward);
        hmDur = Vector3.Distance(startPos, endPos) / speed;

        while (t < hmDur)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t / hmDur);

            t += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;

        onWall = false;
        navMeshAgent.enabled = true;

        characterCollider.enabled = true;

        IMoveOnWallHelper = null;
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
                        joystickWalk = new JoystickWalking(navMeshAgent, transform);
                    if (!onWall)
                        Velocity=joystickWalk.UpdateJoystickWalk();
                }
                else
                {
                    if (randomWalk == null)
                        randomWalk = new RandomWalking(navMeshAgent);
                    if (!onWall)
                        Velocity = randomWalk.UpdateRandomWalk();
                }
            }
            else if(character.Roll == CharacterRoll.Follower && character.Clan != Clan.None)
            {
                if (followLeaderWolking == null)
                    followLeaderWolking = new FollowLeaderWalking(character);

                if (character.leader == null)
                    character.leader = CrowdManager.instance.GetLeader(character.Clan);
                if (!onWall)
                    Velocity = followLeaderWolking.UpdateFollowLeaderWalking(character.leader,CharacterManager.instance.followLeaderType);
            }
            else if(character.Clan == Clan.None)
            {
                if (randomWalk == null)
                    randomWalk = new RandomWalking(navMeshAgent);
                if (!onWall)
                    Velocity = randomWalk.UpdateRandomWalk();
            }

            character.animationController.SetCharacterSpeed(onWall ? 0f : GetSpeedT());

            yield return null;
        }
    }

    #endregion
}
