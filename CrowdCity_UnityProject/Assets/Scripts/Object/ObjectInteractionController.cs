using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionController : MonoBehaviour
{
    #region Public Fields

    public bool interactable = true;

    #endregion

    #region Serialized Fields

    [SerializeField] private List<OnTagTriggerEnter> onTagTriggerEnters;
    [SerializeField] private List<OnTagTriggerExit> onTagTriggerExits;

    [SerializeField] private List<OnTagCollisionEnter> onTagCollisionEnters;
    [SerializeField] private List<OnTagCollisionExit> onTagCollisionExits;

    #endregion

    #region Unity Behaviour

    private void OnTriggerEnter(Collider other)
    {
        if (!interactable)
            return;

        foreach (OnTagTriggerEnter te in onTagTriggerEnters)
        {
            te.CheckAndInvoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!interactable)
            return;

        foreach (OnTagTriggerExit te in onTagTriggerExits)
        {
            te.CheckAndInvoke(other);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!interactable)
            return;

        foreach (OnTagCollisionEnter te in onTagCollisionEnters)
        {
            te.CheckAndInvoke(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!interactable)
            return;

        foreach (OnTagCollisionExit te in onTagCollisionExits)
        {
            te.CheckAndInvoke(collision);
        }
    }

    #endregion

    public void SetInteractableActive(bool value)
    {
        interactable = value;
    }
}
