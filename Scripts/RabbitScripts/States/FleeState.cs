using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    // States
    public StateManager sm;
    public bool canSeePlayer;
    public bool canHearPlayer;
    public State AlertState;
    public GameObject FleeImage;

    // Flee
    private RabbitMetaData rmd;
    private GameObject player;
    private Rigidbody2D rb;
    public float originTime = 4f;
    private float targetTime;
    private Vector2 nextPos;
    private float runningSpeed;

    // Rotate to Player
    private Vector2 lastKnownPosition;
    public GameObject RotateDummy;

    void Start()
    {
        rmd = gameObject.transform.root.gameObject.GetComponent<RabbitMetaData>();
        player = rmd.GetPlayer();
        rb = rmd.GetRigidbody();
        targetTime = originTime;
        runningSpeed = rmd.GetIdleSpeed() * 1.5f;
    }

    public override State RunCurrentState()
    {
        canSeePlayer = sm.GetCanSeePlayer();
        canHearPlayer = sm.GetCanHearPlayer();
        // Run away form player for timer (rabbit is running)
        // Rabbit still checks in viewing direction for player while it runs
        // TIMER RESETS EVERY TIME canSeePlayer == true
        // At end of timer, rabbit is not running
        // Rabit turns to face last known direction of movement of player, updated when canSeePlayer == true
        // Detect for can see player
        nextPos = GetRunningDirection(lastKnownPosition).normalized * runningSpeed;
        rb.drag = 1f * rmd.GetDragMultiplier();
        rb.AddForce(nextPos - rb.velocity, ForceMode2D.Force);
        rmd.SetLastPosDirection(rb.velocity.normalized);
        if(canSeePlayer || canHearPlayer)
        {
            // Change the last known position of the player
            // so that the rabbit runs in the correct direction
            ResetData(rmd.GetPlayer().transform.position);
            FleeImage.SetActive(true);
            // Start/Reset flee state timer
            return this;
        }
        else
        {
            // Run in same direction for 4 seconds
            targetTime -= Time.deltaTime;
            if(targetTime > 0f)
            {
                return this;
            }
            else
            {
                FleeImage.SetActive(false);
                AlertState.ResetData(rmd.GetPlayer().transform.position);
                return AlertState;
            }
        }
    }

    public override void ResetData(Vector2 pos)
    {
        lastKnownPosition = pos;
        targetTime = originTime;
        return;
    }

    // Takes the difference in current rabbit position and current player position
    // Returns the opposite direction so that the rabbit runs in the opposite direction of the player
    // Can be modified to run toward the player, for instance
    private Vector2 GetRunningDirection(Vector2 knownPlayerPosition)
    {
        Vector2 rabbitPosition = rmd.GetGameObject().transform.position;
        Vector2 differenceVector = new Vector2(knownPlayerPosition.x - rabbitPosition.x, knownPlayerPosition.y - rabbitPosition.y) * -1f;
        return differenceVector;
    }
}
