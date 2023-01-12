using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitDamage : MonoBehaviour
{
    private int damage;
    public RabbitDeathStatus deaths;
    public RabbitMetaData rmd;
    public enum DamageType
    {
        Front,
        Side,
        Back
    }
    private int backDamage = 3;
    private int sideDamage = 5;
    private int frontDamage = 7;

    public GameObject bodyParts;

    private string direction = "";

    // Start is called before the first frame update
    void Start()
    {
        damage = 0;
        rmd = gameObject.transform.GetComponent<RabbitMetaData>();
    }

    public void AddDamage(DamageType type)
    {
        int incrementDamage = 0;
        if(type == DamageType.Front)
        {
            incrementDamage = frontDamage;
        }
        else if(type == DamageType.Side)
        {
            incrementDamage = sideDamage;
        }
        else if(type == DamageType.Back)
        {
            incrementDamage = backDamage;
            if(rmd.GetLegs() != RabbitMetaData.LegStatus.None)
            {
                KnockOffLegs();
            }
            rmd.SetLegs(RabbitMetaData.LegStatus.None);
        }
        damage = damage + incrementDamage;
        if(damage >= 10)
        {
            deaths.AddKill();
            if(type == DamageType.Front)
            {
                // Knock off Head
            }
            // Delete after testing
            KnockOffHead();
            gameObject.SetActive(false);
        }
    }

    public void KnockOffLegs()
    {
        DetermineDirection();
        GameObject directionParts = null;
        foreach(Transform child in bodyParts.transform)
        {
            GameObject go = child.gameObject;
            string tag = go.tag;
            if(string.Equals(tag, direction))
            {
                directionParts = go;
            }
        }
        GameObject RightLeg = null;
        GameObject LeftLeg = null;
        foreach(Transform child in directionParts.transform)
        {
            GameObject go = child.gameObject;
            string tag = go.tag;
            if(string.Equals(tag, "RightLeg"))
            {
                RightLeg = go;
            }
            if(string.Equals(tag, "LeftLeg"))
            {
                LeftLeg = go;
            }
        }
        Launch(RightLeg);
        Launch(LeftLeg);
    }

    public void KnockOffHead()
    {
        DetermineDirection();
        GameObject directionParts = null;
        foreach(Transform child in bodyParts.transform)
        {
            GameObject go = child.gameObject;
            string tag = go.tag;
            if(string.Equals(tag, direction))
            {
                directionParts = go;
            }
        }
        GameObject Head = null;
        GameObject Body = null;
        GameObject Full = null;
        foreach(Transform child in directionParts.transform)
        {
            GameObject go = child.gameObject;
            string tag = go.tag;
            if(string.Equals(tag, "Head"))
            {
                Head = go;
            }
            if(string.Equals(tag, "Body"))
            {
                Body = go;
            }
            if(string.Equals(tag, "Full"))
            {
                Full = go;
            }
        }
        Launch(Head);
        Launch(Body);
    }

    private void DetermineDirection()
    {
        Vector2 lastPosDirection = rmd.GetLastPosDirection().normalized;
        int down = 0;
        int up = 0;
        int left = 0;
        int right = 0;
        if(lastPosDirection.y < lastPosDirection.x)
        {
            right = right + 1;
            down = down + 1;
        }
        else
        {
            left = left + 1;
            up = up + 1;
        }
        if(lastPosDirection.y < -1 * lastPosDirection.x)
        {
            left = left + 1;
            down = down + 1;
        }
        else
        {
            up = up + 1;
            right = right + 1;
        }
        if(up == 2)
        {
            direction = "Up";
        }
        else if(left == 2)
        {
            direction = "Left";
        }
        else if(right == 2)
        {
            direction = "Right";
        }
        else direction = "Down";
    }

    private void Launch(GameObject go)
    {
        go.SetActive(true);
        Rigidbody2D rb2 = go.GetComponent<Rigidbody2D>();
        rb2.position = rmd.GetRigidbody().position;
        float initialY = rb2.position.y;
        // Launching Details
        float launchY = Random.Range(100f, 150f);
        float launchX = Random.Range(-100f, 100f);
        Vector2 launchVector = new Vector2(launchX, launchY);
        rb2.AddForce(launchVector);
        float torque = launchX;
        rb2.AddTorque(torque);
        BodyPartMovement bpm = go.GetComponent<BodyPartMovement>();
        bpm.SetFreeze(initialY, rb2);
    }

    // Coroutine to freeze y direction after a time
}
