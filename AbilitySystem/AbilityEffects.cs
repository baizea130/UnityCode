using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffectsController : ScriptableObject
{
    public abstract void Execute(GameObject initiator,GameObject target);
}