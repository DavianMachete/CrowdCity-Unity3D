using System.Collections;
using System.Collections.Generic;
using Machete.Character;
using UnityEngine;

public class BuildingTransparentMaker : TransparentMaker
{
    [Space(40f)]
    [SerializeField] private CharacterRoll characterRoll;

    public override void OnObjectBehindOf(Collider collider)
    {
        Character cc = collider.GetComponent<Character>();
        if (cc == null)
            return;
        if (cc.Roll != characterRoll)
            return;

        base.OnObjectBehindOf(collider);
    }

    public override void OnObjectNotBehindOf(Collider collider)
    {
        Character cc = collider.GetComponent<Character>();
        if (cc == null)
            return;
        if (cc.Roll != characterRoll)
            return;

        base.OnObjectNotBehindOf(collider);
    }
}
