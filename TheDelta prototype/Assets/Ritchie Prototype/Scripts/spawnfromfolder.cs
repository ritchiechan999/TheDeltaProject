using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnfromfolder : MonoBehaviour
{
    public bool on;

    public SpawnSkillScriptableObject testSpawn;
    public string teststring;
    // Start is called before the first frame update
    void Start()
    {
        //temporary
        
    }

    // Update is called once per frame
    void Update()
    {
        testSpawn = Resources.Load("SkillEff/" + teststring) as SpawnSkillScriptableObject;
        if (on)
        {
            //Instantiate(Resources.Load("SkillEff/Combo1") as GameObject);
            Instantiate(testSpawn.SkillPrefabs.gameObject);
        }
    }
}
