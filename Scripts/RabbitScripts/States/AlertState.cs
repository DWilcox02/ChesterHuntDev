using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : State
{
    // States
    public StateManager sm;
    public bool canSeePlayer;
    public bool canHearPlayer;
    public State IdleState;
    public State FleeState;
    public GameObject AlertImage;

    // Alertness
    private RabbitMetaData rmd;
    private GameObject player;
    private Rigidbody2D rb;
    public float originTime = 4f;
    private float targetTime;

    // Rotate to player
    private Vector2 lastKnownPosition;
    public GameObject RotateDummy;
    private float originTimeRotate = 0.75f;
    private float targetTimeRotate;
    private bool isCurious;

    void Start()
    {
        rmd = gameObject.transform.root.gameObject.GetComponent<RabbitMetaData>();
        player = rmd.GetPlayer();
        rb = rmd.GetRigidbody();
        targetTime = originTime;
        targetTimeRotate = originTimeRotate;
        isCurious = true;
    }

    public override State RunCurrentState()
    {
        AlertImage.SetActive(true);
        // What to do when alert
        canSeePlayer = sm.GetCanSeePlayer();
        canHearPlayer = sm.GetCanHearPlayer();
        if(canSeePlayer)
        {
            // Reset alert timers back to origin
            AlertImage.SetActive(false);
            lastKnownPosition = rmd.GetPlayer().transform.position;
            ResetData(lastKnownPosition);
            FleeState.ResetData(lastKnownPosition);
            return FleeState;
        }
        else if(canHearPlayer)
        {
            // Restart alert state timer, because can still hear the player
            lastKnownPosition = rmd.GetPlayer().transform.position;
            ResetData(lastKnownPosition);
            AlertImage.SetActive(true);
            return this;
        }
        else
        {
            // Cannot hear or see player
            // Continue timer
            // Has now turned to face player, is looking in last known direction
            if(!isCurious)
            {
                targetTime -= Time.deltaTime;
                if(targetTime > 0f)
                {
                    return this;
                }
                else
                {
                    AlertImage.SetActive(false);
                    return IdleState;
                }
            }
            // Is curious. Will eliminate timer for a few moments of buffer first, then change position and go through rest
            else
            {
                targetTimeRotate -= Time.deltaTime;
                if(targetTimeRotate > 0f)
                {
                    return this;
                }
                else
                {
                    isCurious = false;
                    Vector2 currentDirection = rmd.GetLastPosDirection().normalized;
                    Vector2 currentPosition = rmd.GetGameObject().transform.position;
                    Vector2 playerDirection = new Vector2(lastKnownPosition.x - currentPosition.x, lastKnownPosition.y - currentPosition.y);
                    rmd.SetLastPosDirection(playerDirection);
                    return this;
                }
            }
        }
    }

    public override void ResetData(Vector2 pos)
    {
        lastKnownPosition = pos;
        targetTime = originTime;
        isCurious = true;
        targetTimeRotate = originTimeRotate;
    }
}
