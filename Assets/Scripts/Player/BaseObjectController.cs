using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 2f;
    public float attackSpeed = 1f;
    
    protected bool isLive_ = true;
    protected bool isAttacking_ = false;
    protected bool canMoving_ = false;

    protected float moveH_;
    protected float moveV_;
    protected Rigidbody rigidbody_;

    protected Animator animator_;

    // Start is called before the first frame update
    void Start()
    {
        animator_ = GetComponent<Animator>();
        rigidbody_ = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveLogic();
    }

    protected virtual void attackLogic()
    {
        animator_.SetTrigger("Attack");
        canMoving_ = false;
    }
    
    public virtual void move(float moveH,float moveV)
    {
        moveH_ = moveH;
        moveV_ = moveV;
    }

    public virtual void aliveLogic()
    {
        
    }

    protected virtual void moveLogic()
    {
        transform.Rotate(0, moveH_ * Time.deltaTime * rotateSpeed, 0);
        transform.Translate(transform.forward * moveV_ * Time.deltaTime * moveSpeed, Space.World);
    }

    protected virtual void deathLogic()
    {
        animator_.SetTrigger("Death");
    }

}
