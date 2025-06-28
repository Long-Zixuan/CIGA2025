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
    public float damage;
    public float attackForce = 1f;
   
    public BoxCollider attackCol;
    [Header("Alive")] public float aliveOffect = 1f;

    [Header("Render")] public Renderer render_;
    
    protected bool isDie_ = true;
    protected bool isAttacking_ = false;
    protected bool canMoving_ = false;
    protected bool isAlive_ = false;

    public bool IsAlive
    {
        get { return isAlive_; }
    }

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

    public void setOutlineCol(Color col)
    {
        render_.material.SetColor("_OutlineColor", col);
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

    public virtual Vector3 getForword()
    {
        return transform.right;
    }
    
    public virtual void move(float moveH,float moveV)
    {
        moveH_ = moveH;
        moveV_ = moveV;
    }

    public virtual void aliveLogic(BasePlayerController playerController)
    {
        playerController_ = playerController;
        transform.rotation = Quaternion.Euler(0, playerController.transform.rotation.eulerAngles.y, 0);
        transform.position = playerController.transform.position + new Vector3(0, aliveOffect, 0);
       // rigidbody_.freezeRotation = true;
        rigidbody_.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        isAlive_ = true;
    }

    public virtual void deAliveLogic()
    {
        rigidbody_.constraints = RigidbodyConstraints.None;
        playerController_ = null;
        isAlive_ = false;
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
