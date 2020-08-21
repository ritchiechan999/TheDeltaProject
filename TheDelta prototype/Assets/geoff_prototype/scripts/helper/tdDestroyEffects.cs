using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdDestroyEffects : MonoBehaviour
{
    public float DestroyTimer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, DestroyTimer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
