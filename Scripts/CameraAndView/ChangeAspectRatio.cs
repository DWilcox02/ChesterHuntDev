using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAspectRatio : MonoBehaviour
{
    private RectTransform rt;
    private float width;
    private float height;
    private int count = 0;
    private int num = 100;

    void Start()
    {
        rt = gameObject.transform.parent.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void FixedUpdate()
    {
        if(count < num)
        {
            width = rt.rect.width;
            height = rt.rect.height;
            gameObject.transform.localScale = new Vector3(0.07f * width, 0.11f * height, 4.35f);
            count++;
        }
        
    }
}
