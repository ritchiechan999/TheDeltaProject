using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effects", menuName = "TheDeltas/Skill", order = 1)]
public class tdEffects : ScriptableObject
{    
    public string SkillName;
    public tdFX Type;
    public GameObject SkillPrefab;

    public Vector3 PosRightOffset;
    public Vector3 PosLeftOffset;
    public Vector3 RotRightOffset;
    public Vector3 RotLeftOffset;
}

