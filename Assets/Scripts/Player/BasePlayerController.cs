using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerController : MonoBehaviour
{
    [Header("Key")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode attackKey = KeyCode.Mouse0;
    [Header("HP")]
    public float beginHp = 10;

    protected float hp_;
    
    private BaseObjectController objectController_;
    
    protected bool isDie_= false;

    public BaseObjectController ObjectController
    {
        set
        {
            objectController_ = value;
        }
    }
    
    // Start is called before the first frame update
    protected void Start()
    {
        objectController_ = GetComponent<BaseObjectController>();
        hp_ = beginHp;
    }

    // Update is called once per frame
    protected void Update()
    {
        moveLogic();
        if (Input.GetKeyDown(attackKey))
        {
            objectController_.attack();
        }

        if (hp_ <= 0)
        {
            dieLogic();
        }
    }
    
    public virtual void beBeating(float othDamage)
    {
        hp_ -= othDamage;
        print(gameObject.name+":HP="+hp_);
    }

    protected void dieLogic()
    {
        if (!isDie_)
        {
            objectController_.deathLogic();
            isDie_ = true;
        }
    }
    
    protected void moveLogic()
    {
        float moveV = 0;
        float moveH = 0;
        if (Input.GetKey(upKey))
        {
            moveV += 1;
        }
        if (Input.GetKey(downKey))
        {
            moveV -= 1;
        }
        if (Input.GetKey(leftKey))
        {
            moveH -= 1;
        }

        if (Input.GetKey(rightKey))
        {
            moveH += 1;
        }
        
        objectController_.move(moveH, moveV);
        
    }
    
    
}
