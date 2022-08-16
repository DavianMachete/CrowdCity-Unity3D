using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Machete.Character
{
    public class CharacterCollisionController : MonoBehaviour
    {
        private Character controller;
        private int checkingCountInAFrame;
        private float seekingDistance;
        private List<Character> opponentAndFreeCharacters;
        private List<Crowd> opponetsCrowd;

        public void Prepare(Character controller, float seekingDistance, int checkingCountInAFrame)
        {
            if (opponentAndFreeCharacters == null)
                opponentAndFreeCharacters = new List<Character>();
            opponentAndFreeCharacters.Clear();

            if (opponetsCrowd == null)
                opponetsCrowd = new List<Crowd>();
            opponetsCrowd.Clear();

            this.controller = controller;
            this.seekingDistance = seekingDistance;
            this.checkingCountInAFrame = checkingCountInAFrame;

            StopDetecting();
        }

        public void StartDetecting()
        {
            find = true;
            if (IFindToDetectHelper == null)
                IFindToDetectHelper = StartCoroutine(IFindToDetect());
        }

        public void StopDetecting()
        {
            find = false;
            if (IFindToDetectHelper != null)
                StopCoroutine(IFindToDetectHelper);
            IFindToDetectHelper = null;
        }

        private bool find = false;
        private Coroutine IFindToDetectHelper;
        private IEnumerator IFindToDetect()
        {
            int checkedCount = 0;
            while (find)
            {
                //GetOpponentAndFreeCharacters
                opponentAndFreeCharacters.Clear();

                for (int i = 0; i < CrowdManager.instance.FreeCharacters.Count; i++)
                {
                    if (checkedCount > checkingCountInAFrame)
                    {
                        checkedCount = 0;
                        yield return null;
                    }
                    checkedCount++;


                    opponentAndFreeCharacters.Add(CrowdManager.instance.FreeCharacters[i]);

                }

                foreach (Crowd crowd in CrowdManager.instance.Crowds)
                {
                    if (crowd.clan != controller.Clan)
                    {
                        opponentAndFreeCharacters.Add(crowd.leader);
                        checkedCount += 2;
                        for (int i = 0; i < crowd.followers.Count; i++)
                        {
                            if (checkedCount > checkingCountInAFrame)
                            {
                                checkedCount = 0;
                                yield return null;
                            }
                            checkedCount++;

                            opponentAndFreeCharacters.Add(crowd.followers[i]);
                        }
                    }
                }
                //Check

                for (int i = 0; i < opponentAndFreeCharacters.Count; i++)
                {
                    if (checkedCount > checkingCountInAFrame)
                    {
                        checkedCount = 0;
                        yield return null;
                    }
                    checkedCount++;

                    if (Vector3.Distance(transform.position, opponentAndFreeCharacters[i].transform.position) < seekingDistance)
                        controller.OnInteractWithCharacter(opponentAndFreeCharacters[i]);
                }

                yield return null;
            }
        }
    }
}