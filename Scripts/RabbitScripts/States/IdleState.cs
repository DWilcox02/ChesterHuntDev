using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    // States
    public StateManager sm;
    public bool canSeePlayer; //pr
    public bool canHearPlayer; //pr
    public State FleeState;
    public State AlertState;
    public GameObject IdleImage;
    public GameObject FleeImage;

    // Changing State
    private float originTimeDelay = 0.15f;
    private float targetTimeDelay;

    // Movement
    public RabbitMetaData rmd;  //pr
    public Vector2 nextPos; //pr
    private float speed;
    public bool isWaiting = false; //pr
    public Rigidbody2D rb; //pr

    void Start()
    {
        rmd = gameObject.transform.root.gameObject.GetComponent<RabbitMetaData>();
        nextPos = RandomPos();
        rb = rmd.GetRigidbody();
        speed = rmd.GetIdleSpeed();
        targetTimeDelay = originTimeDelay;
    }

    Vector2 RandomPos(){
        float x = Random.Range(-5.0f, 5.0f);
        float y = Random.Range(-5.0f, 5.0f);
        return new Vector3(x, y);
    }

    public override State RunCurrentState()
    {
        IdleImage.SetActive(true);
        if(!isWaiting) //check if waiting == false
        {
            float xDiff = Mathf.Abs(rmd.GetGameObject().transform.position.x - nextPos.x);
            float yDiff = Mathf.Abs(rmd.GetGameObject().transform.position.y - nextPos.y);
            if (xDiff > 0.05 && yDiff > 0.05)
            {
                rb.drag = 1f * rmd.GetDragMultiplier();
                Vector2 newVel = new Vector2(nextPos.x - rmd.GetGameObject().transform.position.x, nextPos.y - rmd.GetGameObject().transform.position.y).normalized * speed;
                rb.AddForce(newVel - rb.velocity, ForceMode2D.Force);
                if(!(newVel.x == 0 && newVel.y == 0))
                {
                    rmd.SetLastPosDirection(rb.velocity.normalized);
                }
            }
            else
            {
                StartCoroutine(Wait()); // start the coroutine to wait
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        // State change logic
        canSeePlayer = sm.GetCanSeePlayer();
        canHearPlayer = sm.GetCanHearPlayer();
        if(canSeePlayer)
        {
            // Starts flee timer for flee state
            // Reset alertstate timers
            IdleImage.SetActive(false);
            FleeState.ResetData(rmd.GetPlayer().transform.position);
            FleeImage.SetActive(true);
            targetTimeDelay -= Time.deltaTime;
            if(targetTimeDelay > 0f)
            {
                return this;
            }
            FleeImage.SetActive(false);
            return FleeState;
        }
        else if(canHearPlayer)
        {
            rb.velocity = Vector2.zero;
            AlertState.ResetData(rmd.GetPlayer().transform.position);
            IdleImage.SetActive(false);
            targetTimeDelay = originTimeDelay;
            return AlertState;
        }
        else
        {
            targetTimeDelay = originTimeDelay;
            return this;
        }
    }

    IEnumerator Wait()
    {  
        isWaiting = true;  //set the bool to stop moving
        rb.velocity = new Vector2(0f, 0f);
        rb.drag = 3f * rmd.GetDragMultiplier();
        float seconds = Random.Range(2f, 8f);
        yield return new WaitForSeconds(seconds); // wait for 5 sec
        nextPos = RandomPos();
        isWaiting = false; // set the bool to start moving
    }

    public override void ResetData(Vector2 pos)
    {
        targetTimeDelay = originTimeDelay;
        return;
    }
}
