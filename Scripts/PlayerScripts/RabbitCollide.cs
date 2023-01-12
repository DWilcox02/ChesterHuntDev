using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitCollide : MonoBehaviour
{
    public PlayerMovement pm;
    public StateManager sm;
    private float cooldown;
    private float timeStamp;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0.5f;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        GameObject go = other.gameObject;
        if(go.tag == "Rabbit")
        {
            RabbitCollision(go);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        GameObject go = other.gameObject;
        if(go.tag == "HearingCollider")
        {
            HearingCollision(go);
        }
        else if(go.tag == "VisionCollider")
        {
            VisionCollision(go);
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        GameObject go = other.gameObject;
        if(go.tag == "HearingCollider")
        {
            sm.SetCanHearPlayer(false);
        }
        else if(go.tag == "VisionCollider")
        {
            sm.SetCanSeePlayer(false);
        }
    }

    void RabbitCollision(GameObject go)
    {
        RabbitMetaData rmd = go.GetComponent<RabbitMetaData>();
        Vector2 playerDir = pm.GetLastPosDirection().normalized;
        Vector2 rabbitDir = rmd.GetLastPosDirection().normalized;
        float angle = DetermineAngle(playerDir, rabbitDir);
        if(timeStamp <= Time.time)
        {
            DoDamage(angle, go.GetComponent(typeof(RabbitDamage)) as RabbitDamage);
        }
    }

    float DetermineAngle(Vector2 player, Vector2 rabbit)
    {
        float cos = ((player.x * rabbit.x) + (player.y * rabbit.y)) / (Mathf.Sqrt((player.x*player.x + player.y*player.y) * (rabbit.x*rabbit.x + rabbit.y*rabbit.y)));
        return Mathf.Acos(cos);
    }

    void DoDamage(float angle, RabbitDamage rd)
    {
        RabbitDamage.DamageType type = RabbitDamage.DamageType.Front;
        if(angle < 5.49779)
        {
            if(angle < 3.92699)
            {
                if(angle < 2.35619)
                {
                    // Back
                    if(angle < 0.785398)
                    {
                        type = RabbitDamage.DamageType.Back;
                    }
                    // Side
                    else
                    {
                        type = RabbitDamage.DamageType.Side;
                    } 
                }
                // Front
                else
                {
                    type = RabbitDamage.DamageType.Front;
                }
            }
            // Side
            else
            {
                type = RabbitDamage.DamageType.Side;
            } 
        }
        // Back
        else type = RabbitDamage.DamageType.Back;
        rd.AddDamage(type);
        timeStamp = Time.time + cooldown;
    }

    void HearingCollision(GameObject go)
    {
        GameObject rabbit = go.transform.parent.gameObject;
        float velocity = pm.GetVelocity().magnitude;
        if(pm.GetVelocity().magnitude > 1.2)
        {
            sm.SetCanHearPlayer(true);
        }
        else sm.SetCanHearPlayer(false);
    }

    void VisionCollision(GameObject go)
    {
        GameObject rabbit = go.transform.parent.gameObject.transform.parent.gameObject;
        bool playerIsHidden = pm.GetHiddenBool();
        RabbitMetaData rmd = rabbit.GetComponent<RabbitMetaData>();
        bool rabbitIsHidden = rmd.GetHiddenBool();
        if(playerIsHidden == rabbitIsHidden)
        {
            sm.SetCanSeePlayer(true);
        }
        else sm.SetCanSeePlayer(false);
    }
}
