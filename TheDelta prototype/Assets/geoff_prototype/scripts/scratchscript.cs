using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scratchscript : MonoBehaviour
{
    public AnimationClip AnimClip;

    public float ClipTime;

    private void Start() {
        ClipTime = AnimClip.length;
    }
}
