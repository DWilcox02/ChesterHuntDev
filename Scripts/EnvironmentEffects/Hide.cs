using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hide : MonoBehaviour
{

    public GameObject SneakOverlay;
    private SpriteRenderer sp;
    private float slowDown = 4f;
    

    void Start()
    {
        SneakOverlay.SetActive(false);
        sp = gameObject.transform.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        GameObject go = other.gameObject;
        if(go.tag == "HidingCollider")
        {
            GameObject parent = go.transform.root.gameObject;
            if(parent.tag == "Player")
            {
                PlayerMovement mp = parent.GetComponent(typeof(PlayerMovement)) as PlayerMovement;
                mp.ProductMultiplier(slowDown);
                mp.SetHiddenBool(true);
                SneakOverlay.SetActive(true);
                sp.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }
            if(parent.tag == "Rabbit")
            {
                RabbitMetaData rmd = parent.GetComponent<RabbitMetaData>();
                rmd.SetHiddenBool(true);
                rmd.MultiplyDragMultiplier(slowDown * 2f);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        GameObject go = other.gameObject;
        if(go.tag == "HidingCollider")
        {
            GameObject parent = go.transform.parent.gameObject;
            if(parent.tag == "Player")
            {
                PlayerMovement mp = parent.GetComponent(typeof(PlayerMovement)) as PlayerMovement;
                mp.ProductMultiplier((1f / slowDown));
                mp.SetHiddenBool(false);
                SneakOverlay.SetActive(false);
                sp.color = new Color(1.0f, 1.0f, 1.0f, 1f);
            }
            if(parent.tag == "Rabbit")
            {
                RabbitMetaData rmd = parent.GetComponent<RabbitMetaData>();
                rmd.SetHiddenBool(false);
                rmd.MultiplyDragMultiplier((0.5f / slowDown));
            }
        }
    }
}
