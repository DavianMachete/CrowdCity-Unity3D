using System.Collections;
using System.Collections.Generic;
using Machete.Character;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public static CrowdManager instance;

    public int CharactersCount { get { return CharactersCountInCrowds() + FreeCharacters.Count; } }
    public int CharactersMaxLimit { get { return charactersMaxLimit; } }

    [HideInInspector] public List<Character> FreeCharacters;
    public List<Crowd> Crowds;

    public List<CrowdMaterial> crowdMaterials;

    [SerializeField] private GameObject characterPrefab;

    [SerializeField] private int charactersMaxLimit = 2000;
    [SerializeField] private int followersStartCount = 100;
    [SerializeField] private int crowdsCount = 4;
    [SerializeField] private int freeCharactersCountAtStart = 8;
    [SerializeField] private float freeCharacterGenerationDelay = 4f;
    [SerializeField] private int freeCharactersCountOnGeneration = 2;
    //[Space]
    //[SerializeField] private int seekingsInOneFrame = 200;
    //[SerializeField] private float seekingCollisonDistance = 1f;

    //private List<CharacterController> characters;


    private void Awake()
    {
        instance = this;
    }



    public void Initialize()
    {
        if(crowdsCount > System.Enum.GetNames(typeof(Clan)).Length - 1)
        {
            Debug.LogWarning("Crowds Count cant be more that Clans known to us count");
            crowdsCount = System.Enum.GetNames(typeof(Clan)).Length - 1;
        }

        //StopSeekToDetectCollision();
        StopFreeCharctersGeneration();
        DestroyAllCharacters();

        //if (characters == null)
        //    characters = new List<CharacterController>();
        //characters.Clear();

        if (Crowds == null)
            Crowds = new List<Crowd>();
        Crowds.Clear();

        if (FreeCharacters == null)
            FreeCharacters = new List<Character>();
        FreeCharacters.Clear();

        GenerateCrowds();

        StartFreeCharctersGeneration();
        //StartSeekToDetectCollision();
    }

    public void StartBattle()
    {
        foreach (Crowd crowd in Crowds)
        {
            crowd.leader.StartCharacter();
            foreach (Character follower in crowd.followers)
            {
                follower.StartCharacter();
            }
        }

        foreach (Character freeCharacter in FreeCharacters)
        {
            freeCharacter.StartCharacter();
        }
    }

    public void SetCrowdsCount(float count)
    {
        crowdsCount = Mathf.FloorToInt(count);
    }

    public void SetFreeCharactersCountAtStart(float count)
    {
        freeCharactersCountAtStart = Mathf.FloorToInt(count);
    }

    public void SetFreeCharacterGenerationDelay(float delay)
    {
        freeCharacterGenerationDelay = delay;
    }

    public void SetFreeCharactersCountOnGeneration(float count)
    {
        freeCharactersCountOnGeneration = Mathf.FloorToInt(count);
    }

    public void SetMaximumCharactersCountLimit(float count)
    {
        charactersMaxLimit = Mathf.FloorToInt(count);
    }

    //public void SetSeekingsCountInOneFrame(float count)
    //{
    //    seekingsInOneFrame = Mathf.FloorToInt(count);
    //}

    //public void SetSeekingsCollisionDistance(float distance)
    //{
    //    seekingCollisonDistance = distance;
    //}

    public Character GetLeader(Clan clan)
    {
        return GetCrowd(clan).leader;
    }

    public Character InstantiateCharacter(Clan clan,CharacterRoll roll,bool startMovement = false)
    {
        return InstantiateCharacter(clan, roll, EnviromentManager.instance.GetRandomPosInArea(), startMovement);
    }

    public Character InstantiateCharacter(Clan clan, CharacterRoll roll,Vector3 position, bool startMovement = false)
    {
        GameObject newCharacter = Instantiate(characterPrefab, Utilities.GetPointByRayCast(position), Quaternion.identity);

        string name = clan.ToString();
        if (roll == CharacterRoll.Follower && clan != Clan.None)
            name += $"'s follower {GetCrowd(clan).followers.Count + 1}";
        else if (clan == Clan.None)
            name = $"free characters = {FreeCharacters.Count + 1}";
        newCharacter.name = name;

        Character newCC = newCharacter.GetComponent<Character>();

        if (roll == CharacterRoll.Leader)
            newCC.SetMaterial(GetCrowdLeaderMaterial(clan));
        else
            newCC.SetMaterial(GetCrowdMaterial(clan));

        newCC.InitializeCharacter(clan, roll, startMovement);

        return newCC;
    }

    public Material GetCrowdMaterial(Clan clan)
    {
        foreach (CrowdMaterial cm in crowdMaterials)
        {
            if (cm.clan == clan)
                return cm.material;
        }
        return null;
    }

    public Material GetCrowdLeaderMaterial(Clan clan)
    {
        foreach (CrowdMaterial cm in crowdMaterials)
        {
            if (cm.clan == clan)
                return cm.leaderMaterial;
        }
        return null;
    }

    public Crowd GetCrowd(Clan clan)
    {
        foreach (Crowd c in Crowds)
        {
            if (c.clan == clan)
                return c;
        }
        return null;
    }

    public void RemoveCrowd(Clan clan)
    {
        Crowd c = GetCrowd(clan);
        //c.DestroyCrowd();
        Crowds.Remove(c);
    }

    public void RemoveFreeCharacter(Character characterController)
    {
        FreeCharacters.Remove(characterController);
    }

    public int CharactersCountInCrowds()
    {
        int i = 0;
        foreach (var c in Crowds)
        {
            i += c.Count;
        }
        return i;
    }


    private void GenerateCrowds()
    {
        for (int i = 1; i < crowdsCount+1; i++)
        {
            Character leader = InstantiateCharacter((Clan)i, CharacterRoll.Leader);
            if (i == 1)
            {
                PlayerController.instance.SetPlayer(leader);
            }

            //characters.Add(leader);
            Crowd newCrowd = new Crowd(leader);
            Crowds.Add(newCrowd);
            leader.PrepareCrowdCounter();

            for (int j = 0; j < followersStartCount; j++)
            {
                Character follower =
                    InstantiateCharacter((Clan)i,CharacterRoll.Follower,
                    leader.transform.position + CharacterManager.instance.positionVectors[newCrowd.followers.Count]);
                newCrowd.AddFollower(follower);
            }
        }
    }

    private void GenerateFreeCharacters(int count, bool startMovement = false)
    {
        int dif = charactersMaxLimit - CharactersCount;//characters.Count;
        if (dif <= 0)
            return;
        else if (count > dif)
        {
            count = dif;
        }

        for (int i = 0; i < count ; i++)
        {
            Character freeCharacter = InstantiateCharacter(0, CharacterRoll.Free, startMovement);
            FreeCharacters.Add(freeCharacter);
            //characters.Add(freeCharacter);
        }

        Debug.Log($"CharactersCount = {CharactersCount}");
        //Debug.Log($"characters.Count = {characters.Count}");
    }

    private void DestroyAllCharacters()
    {
        if (Crowds != null)
        {
            foreach (Crowd c in Crowds)
            {
                c.DestroyCrowd();
            }
        }

        if (FreeCharacters != null)
        {
            foreach (Character cc in FreeCharacters)
            {
                cc.DestroyCharacter();
            }
        }
    }

    private void StartFreeCharctersGeneration()
    {
        generate = true;
        if (IGenerateFreeCharactersHelper == null)
            IGenerateFreeCharactersHelper = StartCoroutine(IGenerateFreeCharacters());
    }

    private void StopFreeCharctersGeneration()
    {
        generate = false;
        if (IGenerateFreeCharactersHelper != null)
            StopCoroutine(IGenerateFreeCharactersHelper);
        IGenerateFreeCharactersHelper = null;
    }

    //private void StartSeekToDetectCollision()
    //{
    //    isSeeking = true;
    //    if (ISeekToDetectCollisionHelper == null)
    //        ISeekToDetectCollisionHelper = StartCoroutine(ISeekToDetectCollision());
    //}

    //private void StopSeekToDetectCollision()
    //{
    //    isSeeking = false;
    //    if (ISeekToDetectCollisionHelper != null)
    //        StopCoroutine(ISeekToDetectCollisionHelper);
    //}

    #region Coroutines

    private bool generate = false;
    private Coroutine IGenerateFreeCharactersHelper;
    private IEnumerator IGenerateFreeCharacters()
    {
        GenerateFreeCharacters(freeCharactersCountAtStart);
        float time = 0f;

        yield return new WaitUntil(() => GameManager.instance.isGameStarted);

        while (generate)
        {
            if (time > freeCharacterGenerationDelay)
            {
                time = 0f;
                GenerateFreeCharacters(freeCharactersCountOnGeneration, true);
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

    //private bool isSeeking = false;
    //private Coroutine ISeekToDetectCollisionHelper;
    //private IEnumerator ISeekToDetectCollision()
    //{
    //    yield return new WaitUntil(() => GameManager.instance.isGameStarted);

    //    int seekingsCounter = 0;
    //    while (isSeeking)
    //    {
    //        for (int i = 0; i < characters.Count-1; i++)
    //        {
    //            for (int j = i+1; j < characters.Count; j++)
    //            {
    //                seekingsCounter++;
    //                if(seekingsCounter>seekingsInOneFrame)
    //                {
    //                    seekingsCounter = 0;
    //                    yield return null;
    //                }
    //                if (Vector3.Distance(characters[i].transform.position, characters[j].transform.position) < seekingCollisonDistance)
    //                    characters[i].OnInteractWithCharacter(characters[j]);
    //            }
    //        }

    //        yield return null;
    //    }
    //}

    #endregion
}
