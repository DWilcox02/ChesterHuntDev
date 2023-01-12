using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartMovement : MonoBehaviour
{
    float yFreezeValue = 0f;
    bool tryFreezingValue = false;
    Rigidbody2D rb;

    // Buffer
    float originTime = 0.5f;
    float targetTime;
    bool buffer = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        targetTime = originTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(buffer)
        {
            targetTime -= Time.deltaTime;
            if(targetTime <= 0f)
            {
                buffer = false;
                tryFreezingValue = true;
            }
        }
        if(tryFreezingValue)
        {
            if(rb.position.y <= yFreezeValue)
            {
                Vector2 currentVelocity = rb.velocity;
                rb.velocity = new Vector2(currentVelocity.x, 0f);
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                tryFreezingValue = false;
            }
        }
    }

    public void SetFreeze(float yFreeze, Rigidbody2D rbSetting)
    {
        buffer = true;
        yFreezeValue = yFreeze;
        rb = rbSetting;
    }
}
