using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Vector2 movement;
    private Vector2 lastPosDirection;
    private float dashSpeed;
    private float dashTime;
    private float startDashTime = 0.3f;
    private float dashMultiplier = 2f;
    private Vector2 dashDirection;

    public GameObject pounce;

    public Joystick joystick;
    public Animator animator;

    private Rigidbody2D rb;

    private float dragMultiplier;
    private bool isHidden;

    public Slider speedSlide;

    // Sound Data
    public BellSound Bells;
    public enum SoundStatus
    {
        Quiet,
        Medium,
        Loud
    }
    public SoundStatus sound;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        dashDirection = Vector2.zero;
        dragMultiplier = 1f;
        sound = SoundStatus.Quiet;
    }

    // Update is called once per frame
    // Use to determine input, because based off framerate
    public void JoystickControls()
    {
        float x = joystick.Horizontal;
        float y = joystick.Vertical;
        if(dashDirection != Vector2.zero)
        {
            if(dashTime <= 0.01)
            {
                dashDirection = Vector2.zero;
                dashTime = startDashTime;
                movement.x = x;
                movement.y = y;
                rb.velocity = (movement * speed);
            }
            else
            {
                rb.drag = 1f * dragMultiplier;
                dashTime -= Time.fixedDeltaTime;
                movement = dashDirection * dashSpeed;
            }
        }
        else
        {
            movement.x = x;
            movement.y = y;
        }
        if(!(movement.x == 0 && movement.y == 0))
        {
            lastPosDirection = movement;
            rb.drag = 1f * dragMultiplier;
        }
        else rb.drag = 8f * dragMultiplier;
        
    }

    // FIX! Only change speed and dirction for dash

    void FixedUpdate() {
        JoystickControls();
        rb.AddForce((movement * speed) - rb.velocity, ForceMode2D.Force);
        animator.speed = Mathf.Sqrt(movement.sqrMagnitude);
        animator.SetFloat("HorizontalDir", lastPosDirection.x);
        animator.SetFloat("VerticalDir", lastPosDirection.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        float velMag = rb.velocity.magnitude;
        if(velMag < 0.8)
        {
            sound = SoundStatus.Quiet;
            Bells.SetQuiet();
        }
        else if(velMag < 1.2)
        {
            sound = SoundStatus.Medium;
            Bells.SetMedium();
        }
        else
        {
            sound = SoundStatus.Loud;
            Bells.SetLoud();
        }
        speedSlide.value = velMag;
    }

    public void ControlToPounce(Vector2 newDirection, float radius)
    {
        dashDirection = newDirection;
        dashSpeed = radius / startDashTime * dashMultiplier;
    }

    public void MouseControls()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 input = Input.mousePosition;
            Vector3 location = Camera.main.ScreenToWorldPoint(new Vector3(input.x, input.y, 7.41f));
            Debug.Log(location);
            Vector3 current = rb.position;
            float x = location.x - current.x;
            float y = location.y - current.y;
            movement = new Vector2(x, y);
        }
        else{
            movement = Vector2.zero;
        }
    }

    void KeyboardControls()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // between -1 and 1
        movement.y = Input.GetAxisRaw("Vertical"); // between -1 and 1
    }

    public void ProductMultiplier(float product)
    {
        dragMultiplier = dragMultiplier * product;
    }

    public Vector2 GetLastPosDirection()
    {
        return lastPosDirection;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    public void SetHiddenBool(bool hiddenBool)
    {
        isHidden = hiddenBool;
    }

    public bool GetHiddenBool()
    {
        return isHidden;
    }
}
