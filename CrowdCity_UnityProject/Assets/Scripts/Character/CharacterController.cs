using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    public Clan Clan;
    public CharacterRoll Roll;
    public int indexInCrowd = 0;
    public CharacterMovementController movementController;
    public CharacterAnimationController animationController;
    //public CharacterCollisionController collisionController;
    public CharacterController leader;

    [SerializeField] private List<Renderer> renderers;

    private CrowdCounterController crowdCounter;

    [ContextMenu("Initialize Character")]
    public void InitializeCharacter(Clan clan, CharacterRoll roll, bool startMovement = false)
    {
        Clan = clan;
        Roll = roll;

        movementController.Prepare(this);
        animationController.Prepare(this);

        if (crowdCounter != null)
            crowdCounter.DestroyCounter();


        int characterClanLayer = LayerMask.NameToLayer(clan.ToString());
        gameObject.layer = characterClanLayer;

        if (startMovement)
            StartCharacter();
    }

    public void PrepareCrowdCounter()
    {
        if (Roll == CharacterRoll.Leader)
            crowdCounter = GameManager.instance.gameView.AddCrowdCounter();
        crowdCounter.Prepare(this);
    }

    public void SetMaterial(Material material)
    {
        foreach (var r in renderers)
        {
            r.material = material;
        }
    }

    public void ChangeClan(Clan newClan)
    {
        if (Clan != Clan.None)
        {
            if (Roll == CharacterRoll.Follower)
            {
                if (CrowdManager.instance.GetCrowd(Clan) == null)
                {
                    Debug.Log($"{Clan} is null");
                   // Debug.Break();
                }
                CrowdManager.instance.GetCrowd(Clan)?.RemoveFollower(this);
            }
            else
            {
                if (crowdCounter != null)
                    crowdCounter.DestroyCounter();
                CrowdManager.instance.RemoveCrowd(Clan);
            }
        }
        else
        {
            CrowdManager.instance.RemoveFreeCharacter(this);
        }

        leader = CrowdManager.instance.GetLeader(newClan);
        CrowdManager.instance.GetCrowd(newClan).AddFollower(this);
        InitializeCharacter(newClan, CharacterRoll.Follower, true);
        name = $"{newClan}'s follower {CrowdManager.instance.GetCrowd(newClan).followers.Count + 1}";
        SetMaterial(CrowdManager.instance.GetCrowdMaterial(newClan));
    }


    public void OnInteractWithCharacter(Collision collision)
    {
        CharacterController interacted = collision.gameObject.GetComponent<CharacterController>();
        OnInteractWithCharacter(interacted);
    }

    public bool interactedByAnother = false;
    public void OnInteractWithCharacter(CharacterController interacted)
    {
        if (interactedByAnother ||
            Clan == Clan.None ||
            interacted.Clan == Clan)
            return;

        interacted.interactedByAnother = true;
        interactedByAnother = true;

        Crowd interactedsCrowd = CrowdManager.instance.GetCrowd(interacted.Clan);
        Crowd crowd = CrowdManager.instance.GetCrowd(Clan);

        if (interactedsCrowd == null && crowd != null)
        {
            interacted.ChangeClan(Clan);
        }
        else if(crowd==null && interactedsCrowd != null)
        {
            ChangeClan(interacted.Clan);
        }
        else if(interactedsCrowd != null && crowd != null)
        {
            if (interactedsCrowd.followers.Count > crowd.followers.Count)
            {
                if (Roll != CharacterRoll.Leader ||
                    crowd.followers.Count == 0)
                {
                    ChangeClan(interacted.Clan);
                }
            }
            else if (interactedsCrowd.followers.Count < crowd.followers.Count)
            {
                if (interacted.Roll != CharacterRoll.Leader ||
                    interactedsCrowd.followers.Count == 0)
                {
                    interacted.ChangeClan(Clan);
                }
            }
            else
            {
                if (Clan == Clan.Player)
                {
                    interacted.ChangeClan(Clan);
                }
                else if (interacted.Clan == Clan.Player)
                {
                    ChangeClan(Clan.Player);
                }
                else
                {
                    interacted.ChangeClan(Clan);
                }

            }
        }


        //Debug.Log($"<color=green>{name}</color> interacted with <color=cyan>{interacted.name}</color>");

        interacted.interactedByAnother = false;
        interactedByAnother = false;
    }

    public void StartCharacter()
    {
        //collisionController.StartDetecting();
        movementController.StartUpdateMovement();
    }

    public void DestroyCharacter()
    {
        movementController.StopAllCoroutines();
        if (crowdCounter != null)
            crowdCounter.DestroyCounter();
        Destroy(gameObject);
    }
}
