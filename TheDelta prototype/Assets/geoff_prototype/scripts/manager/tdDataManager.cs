using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdDataManager : MonoBehaviour
{
    public static tdDataManager Instance { private set; get; }
    private void Awake() {
        Instance = this;
    }

    private void OnDestroy() {
        Instance = null;
    }
    //on actual project
    //Dictionary<tdFX, GameObject> _tdEffects = new Dictionary<tdFX, GameObject>();

    //placeholder only
    [Header("Effects")]
    public AttackEffects[] AtkFx;

    public SpawnSkillScriptableObject testSpawn;
    public int teststring;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //testSpawn = Resources.Load("SkillEff/Combo" + teststring) as SpawnSkillScriptableObject;
        
    }

    //TODO change these use dictionary for future use
    public void AttackFx(tdFX effect, tdEntity entity) {
        int comboNb = (int)effect;
        GameObject fx = Instantiate(AtkFx[comboNb].Prefab);
        Vector3 posOffset;
        Quaternion rotOffset;
        if (entity.IsFacingRight) {
            posOffset = AtkFx[comboNb].PosRightOffset;
            rotOffset = Quaternion.Euler(AtkFx[comboNb].RotRightOffset);
        } else {
            posOffset = AtkFx[comboNb].PosLeftOffset;
            rotOffset = Quaternion.Euler(AtkFx[comboNb].RotLeftOffset);
        }
        fx.transform.position = entity.transform.position + posOffset;
        fx.transform.rotation = entity.transform.rotation * rotOffset;
    }
    public void AttackFx2(int skillNumber, tdEntity entity)
    {
        
        teststring = skillNumber;
        testSpawn = Resources.Load("SkillEff/Combo" + teststring) as SpawnSkillScriptableObject;
        GameObject fx = Instantiate(testSpawn.SkillPrefabs.gameObject);
        Vector3 posOffset;
        Quaternion rotOffset;
        if (entity.IsFacingRight)
        {
            posOffset = testSpawn.PosRightOffset;
            rotOffset = Quaternion.Euler(testSpawn.RotRightOffset);
        }
        else
        {
            posOffset = testSpawn.PosLeftOffset;
            rotOffset = Quaternion.Euler(testSpawn.RotLeftOffset);
        }
        fx.transform.position = entity.transform.position + posOffset;
        fx.transform.rotation = entity.transform.rotation * rotOffset;
    }
}

//come up with a better name
public enum tdFX {
    Combo1,
    Combo2,
    Combo3,
}

