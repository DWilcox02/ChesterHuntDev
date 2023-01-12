using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionColliderRotation : MonoBehaviour
{
    private RabbitMetaData rmd;
    private Vector2 lastPosDireciton;

    void Start()
    {
        rmd = gameObject.transform.root.gameObject.GetComponent<RabbitMetaData>();
    }

    void Update() {
        lastPosDireciton = rmd.GetLastPosDirection().normalized;
        float w = Mathf.Atan(lastPosDireciton.y / lastPosDireciton.x) * Mathf.Rad2Deg + 90f;
        if(float.IsNaN(w))
        {
            w = 0f;
        }
        if(lastPosDireciton.x < 0)
        {
            w += 180f;
        }
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, w);
    }
}
