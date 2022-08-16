using System;
using System.Collections;
using System.Collections.Generic;
using Machete.Character;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Crowd 
{
    public string crowdName;
    public Clan clan;
    public Character leader;
    public List<Character> followers;

    public UnityAction<int> OnCountUpdated;
    public int Count { get { return followers.Count + 1; } }

    public Crowd (Character leader)
    {
        leader.indexInCrowd = 0;
        followers = new List<Character>();
        crowdName = $"{leader.name}'s Crowd";
        clan = leader.Clan;

        this.leader = leader;
    }

    public void RemoveFollower(Character character)
    {
        followers.Remove(character);
        OnCountUpdated?.Invoke(Count);
        UpdateCrowdIndexes();
    }

    public void AddFollower(Character character)
    {
        followers.Add(character);
        OnCountUpdated?.Invoke(Count);
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
