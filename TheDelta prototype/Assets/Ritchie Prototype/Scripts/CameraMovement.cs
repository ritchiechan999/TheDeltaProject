using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public GameObject Cam;
    public float CamPan;

    public Transform Target;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        CameraPan();
    }
    void Update()
    {
        //Cam.transform.position = Vector3.Lerp(new Vector3(this.transform.position.x, this.transform.position.z, 0), new Vector3(Cam.transform.position.x, Cam.transform.position.z, -10), 1);
    }
    public void CameraPan()
    {
        Vector3 desirePos = Target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desirePos, CamPan * Time.deltaTime);
        this.transform.position = smoothPos;
    }
}
