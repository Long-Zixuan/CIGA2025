using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 2f;
    [Header("Attack")]
    public float attackSpeed = 1f;
    public int damage;
    public float attackForce = 1f;
   
    public BoxCollider attackCol;
    
    protected bool isDie_ = true;
    protected bool isAttacking_ = false;
    protected bool canMoving_ = false;

    protected float moveH_;
    protected float moveV_;
    protected Rigidbody rigidbody_;

    protected Animator animator_;

    protected BasePlayerController playerController_;
    
    public BasePlayerController PlayerController
    {
        get
        {
            return playerController_;
        }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        animator_ = GetComponent<Animator>();
        rigidbody_ = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected void Update()
    {
        moveLogic();
    }

    public virtual void attack()
    {
        animator_.SetTrigger("Attack");
        canMoving_ = false;
    }
    
    public virtual void move(float moveH,float moveV)
    {
        moveH_ = moveH;
        moveV_ = moveV;
    }

    public virtual void aliveLogic(BasePlayerController playerController)
    {
        playerController_ = playerController;
    }

    

    protected virtual void moveLogic()
    {
        if ( moveV_ != 0)
        {
            animator_.SetBool("isMoving", true);
        }
        else
        {
            animator_.SetBool("isMoving", false);
        }
        transform.Rotate(0, moveH_ * Time.deltaTime * rotateSpeed, 0);
        transform.Translate(transform.right * moveV_ * Time.deltaTime * moveSpeed, Space.World);
    }

    public virtual void deathLogic()
    {
        animator_.SetTrigger("Death");
    }

}
