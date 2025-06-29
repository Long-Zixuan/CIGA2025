using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource walkAudio_;
    public AudioSource attackAudio_;
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

    [Header("Effect")] 
    public GameObject aliveEffect;
    public float aliveEffectTime = 1f;
    
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
        if (isAlive_)
        {
            rigidbody_.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            rigidbody_.constraints = RigidbodyConstraints.None;
        }
    }

    public void setOutlineCol(Color col)
    {
        render_.material.SetColor("_OutlineColor", col);
    }

    // Update is called once per frame
    protected void Update()
    {
        moveLogic();
        timer_ += Time.deltaTime;
    }

    protected float timer_ = 0;
    public virtual void attack()
    {
        if (timer_ < attackSpeed)
        {
            return;
        }
        timer_ = 0;
        animator_.SetTrigger("Attack");
        attackAudio_.Play();
        //canMoving_ = false;
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
        transform.position = this.transform.position + new Vector3(0, aliveOffect, 0);
        playerController_.camera_.gameObject.GetComponent<CameraLogic>().player = this.transform;
       // rigidbody_.freezeRotation = true;
       if (rigidbody_ != null)
       {
           rigidbody_.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
       }

       Instantiate(aliveEffect, transform.position + new Vector3(0, aliveEffectTime, 0),
           aliveEffect.transform.rotation);
        isAlive_ = true;
    }

    public virtual void deAliveLogic()
    {
        rigidbody_.constraints = RigidbodyConstraints.None;
        playerController_ = null;
        moveV_ = 0;
        moveH_ = 0;
        gameObject.transform.rotation = Quaternion.Euler(45, 0, 0);
        isAlive_ = false;
    }

    

    protected virtual void moveLogic()
    {
        if ( moveV_ != 0)
        {
            animator_.SetBool("isMoving", true);
            if (!walkAudio_.isPlaying)
            {
                walkAudio_.Play();
            }
        }
        else
        {
            animator_.SetBool("isMoving", false);
            walkAudio_.Stop();
        }
        transform.Rotate(0, moveH_ * Time.deltaTime * rotateSpeed, 0);
        transform.Translate(transform.right * moveV_ * Time.deltaTime * moveSpeed, Space.World);
    }

    public virtual void deathLogic()
    {
        animator_.SetBool("Death",true);
        playerController_.CanMove = false;
    }

}
