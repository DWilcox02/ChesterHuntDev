using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPhysics : MonoBehaviour
{
    private Transform parentT;
    private Quaternion parentQ;


    // Start is called before the first frame update
    void Start()
    {
        parentT = gameObject.transform.parent.gameObject.transform;
        parentQ = parentT.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        parentQ = parentT.rotation;
    }
}
