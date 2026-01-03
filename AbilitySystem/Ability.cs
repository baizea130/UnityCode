using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability",menuName = "Scriptable/Ability")]
public class Ability : ScriptableObject
{
    public string labal;
    public AudioSource AbilityMusic;
    [SerializeReference]public List<AbilityEffectsController> effects;
    void OnEnable()
    {
        if (string.IsNullOrEmpty(labal))labal = name;
    }
    public void Execute(GameObject initiator,GameObject target)
    {
        if (effects.Count == 0)return;
        foreach (var item in effects)
        {
            item.Execute(initiator,target);
        }
    }
}
