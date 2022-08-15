using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace Machete.Character
{
    public class CharacterAnimationController : MonoBehaviour
    {
        #region  Public Fields

        #endregion

        #region Serialized Fields

        [SerializeField] private Animator characterAnimator;

        #endregion

        #region Private Fields

        private Character character;

        #endregion

        #region Unity Behaviour
        #endregion

        #region Public Methods

        public void Prepare(Character character)
        {
            this.character = character;
            SetCharacterSpeed(0f);
        }

        public void SetCharacterSpeed(float value)
        {
            //Debug.Log($"ss {value}");
            SetFloat("speed", value);
        }

        #endregion

        #region Privat Methods

        private void SetFloat(string name, float value)
        {
            characterAnimator.SetFloat(name, value);
        }

        private void SetBool(string name, bool value)
        {
            //Debug.Log($"<color=orange> SetBool():</color> called fro {gameObject.name}, bool name- {name}, vale is {value}");
            characterAnimator?.SetBool(name, value);
        }

        private float RandomizeTForBlendTree(int animationsCount)
        {
            return (1f / (animationsCount - 1f)) * Random.Range(0, animationsCount);
        }

        #endregion

        #region Coroutines

        #endregion
    }
}
