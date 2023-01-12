using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PounceArea : MonoBehaviour
{
    public GameObject pivotPoint;
    public GameObject center;
    private Vector2 movement;
    public float radius;
    public float scale;
    private Vector3 originalScale;

    private bool holding = false;
    public PlayerMovement pm;

    public Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = gameObject.transform.localScale;
    }

    void Update(){
        JoystickControls();
    }

    public void JoystickControls()
    {
        float x = joystick.Horizontal;
        float y = joystick.Vertical;
        if(x != 0 && y != 0)
        {
            holding = true;
            x = joystick.Horizontal;
            y = joystick.Vertical;
            movement = new Vector2(x, y);
        }
        else
        {
            LetGo();
            movement = Vector2.zero;
            scale = 0f;
        }
    }

    public void MouseControls()
    {
        if(Input.GetMouseButton(0))
        {
            holding = true;
            Vector3 input = Input.mousePosition;
            Vector3 location = Camera.main.ScreenToWorldPoint(new Vector3(input.x, input.y, 7.41f));
            Vector3 current = pivotPoint.transform.position;
            float x = location.x - current.x;
            if(x < -1)
            {
                x = -1;
            }
            if(x > 1)
            {
                x = 1;
            }
            float y = location.y - current.y;
            if(y < -1)
            {
                y = -1;
            }
            if(y > 1)
            {
                y = 1;
            }
            movement = new Vector2(x, y);
            
        }
        else{
            LetGo();
            movement = Vector2.zero;
            scale = 0f;
        }
    }

    // Perform instantaneous action upon letting go of a hold
    void LetGo()
    {
        if(holding == true)
        {
            pm.ControlToPounce(movement, DetermineScale() * 2f);
            holding = false;
        }
    }

    void KeyboardControls()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // between -1 and 1
        movement.y = Input.GetAxisRaw("Vertical"); // between -1 and 1
    }

    float DetermineScale()
    {
        return Mathf.Sqrt(Mathf.Pow(movement.x, 2f) + Mathf.Pow(movement.y, 2f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(pivotPoint.transform.position.x + movement.x, pivotPoint.transform.position.y + movement.y, 0);
        center.transform.position = newPos;
        float inverseTan = 0f;
        if(movement.x == 0)
        {
            if(movement.y < 0)
            {
                movement.y = -99999999f;
            }
            if(movement.y > 0)
            {
                movement.y = 99999999f;
            }
            inverseTan = Mathf.Atan(movement.y);
        }
        else
        {
            inverseTan = Mathf.Atan(movement.y / movement.x);
            if(movement.x < 0)
            {
                inverseTan = inverseTan + Mathf.PI;
            }
        }
        float degrees = inverseTan * 180 / Mathf.PI + 90f;
        scale = DetermineScale();
        gameObject.transform.localScale = new Vector3(originalScale.x * scale, originalScale.y * scale, originalScale.z);
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, degrees);
    }
}
