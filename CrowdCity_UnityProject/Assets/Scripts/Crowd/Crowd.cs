using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Crowd 
{
    public string crowdName;
    public Clan clan;
    public CharacterController leader;
    public List<CharacterController> followers;

    public int Count { get { return followers.Count + 1; } }

    public Crowd (CharacterController leader)
    {
        this.leader = leader;
        leader.indexInCrowd = 0;
        followers = new List<CharacterController>();
        crowdName = $"{leader.name}'s Crowd";
        clan = leader.Clan;
    }

    public void RemoveFollower(CharacterController character)
    {
        followers.Remove(character);
        UpdateCrowdIndexes();
    }

    public void AddFollower(CharacterController character)
    {
        followers.Add(character);
        UpdateCrowdIndexes();
    }

    public void DestroyCrowd()
    {
        leader.DestroyCharacter();
        foreach (var f in followers)
        {
            f.DestroyCharacter();
        }
        followers.Clear();
    }

    private void UpdateCrowdIndexes()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            followers[i].indexInCrowd = i + 1;
        }
    }
}
