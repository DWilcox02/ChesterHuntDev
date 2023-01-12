using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followObject;

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(followObject.transform.position.x, followObject.transform.position.y, gameObject.transform.position.z);
    }
}
