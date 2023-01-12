using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAnimation : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController fullController;
    public RuntimeAnimatorController bodyController;
    
    private Vector2 lastPosDirection;
    private float velocityMag;
    private RabbitMetaData rmd;
    

    void Start()
    {
        rmd = gameObject.transform.root.gameObject.GetComponent<RabbitMetaData>();
    }
    
    void Update()
    {
        lastPosDirection = rmd.GetLastPosDirection().normalized;
        velocityMag = rmd.GetVelocity().magnitude;
        animator.SetFloat("HorizontalDir", lastPosDirection.x);
        animator.SetFloat("VerticalDir", lastPosDirection.y);
        animator.SetFloat("Speed", velocityMag);
    }

    public void SetAnimator(RabbitMetaData.LegStatus status)
    {
        if(status == RabbitMetaData.LegStatus.Full)
        {
            animator.runtimeAnimatorController = fullController;
        }
        else if(status == RabbitMetaData.LegStatus.None)
        {
            animator.runtimeAnimatorController = bodyController;
        }
    }
}
