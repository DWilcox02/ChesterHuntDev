using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitMetaData : MonoBehaviour
{
    private Vector2 lastPosDirection;
    public Rigidbody2D rb;
    private GameObject rabbit;
    private float dragMultiplier;
    public GameObject player;
    private bool isHidden;
    private float speed = 0.5f;
    public enum LegStatus
    {
        Full,
        None
    }
    private LegStatus legs = LegStatus.Full;
    public RabbitAnimation rabbitAnimation;
    
    // Start is called before the first frame update
    void Start()
    {
        rabbit = gameObject;
        Vector3 startPoint = RandomPos();
        transform.position = startPoint;
        dragMultiplier = 1f;
    }

    Vector3 RandomPos(){
        float x = Random.Range(-5.0f, 5.0f);
        float y = Random.Range(-5.0f, 5.0f);
        return new Vector3(x, y, 0f);
    }

    public void MultiplyDragMultiplier(float product)
    {
        dragMultiplier = dragMultiplier * product;
    }

    public void SetHiddenBool(bool hiddenBool)
    {
        isHidden = hiddenBool;
    }

    public bool GetHiddenBool()
    {
        return isHidden;
    }

    // Getters and Setters
    public Vector2 GetLastPosDirection()
    {
        return lastPosDirection;
    }

    public void SetLastPosDirection(Vector2 direction)
    {
        lastPosDirection = direction;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }

    public GameObject GetGameObject()
    {
        return rabbit;
    }

    public float GetDragMultiplier()
    {
        return dragMultiplier;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public float GetIdleSpeed()
    {
        return speed;
    }

    public void SetLegs(LegStatus set)
    {
        legs = set;
        rabbitAnimation.SetAnimator(set);
    }

    public LegStatus GetLegs()
    {
        return legs;
    }
}
