using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class BasePlayerController : MonoBehaviour
{
    [Header("Key")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode getToyKey = KeyCode.Mouse1;
    [Header("HP")]
    public float beginHp = 10;

    [Header("Other")] 
    public float getToyDis = 1;
    public Camera camera_;

    protected float hp_;
    protected bool hadObjController = false;
    protected bool canMove_ = true;

    public bool CanMove
    {
        set { canMove_ = value; }
    }

    protected bool canCatchToy_ = true;
    
    public bool CanCatchToy
    {
        set { canCatchToy_ = value; }
    }

    public float Hp
    {
        get { return hp_; }
    }
    
    private BaseObjectController objectController_;
    
    protected bool isDie_= false;

    public bool IsDie
    {
        get { return isDie_; }
    }

    protected BaseObjectController[] toys;
    protected BaseObjectController nestToy_ = null;


    public BaseObjectController ObjectController
    {
        set
        {
            objectController_ = value;
            if (objectController_ != null)
            {
                objectController_.aliveLogic( this);
                hadObjController = true;
            }
        } 
    }
    // Start is called before the first frame update
    protected void Start()
    {
        print("Player Start:"+this.gameObject.name);
        //objectController_ = GetComponent<BaseObjectController>();
        if (hp_ == 0)
        {
            hp_ = beginHp;
        }

        toys = GameManager.Instance.Toys;
        //Test
        //objectController_.aliveLogic(this);
        //print("HP:"+hp_);
        //print("Toys:"+toys.Length);
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
        chooseToyLogic();
    }

    
    
    protected void chooseToyLogic()
    {
        if (nestToy_ != null)
        {
            nestToy_.setOutlineCol(Color.black);
        }
        nestToy_ = null;
        float minDis = float.MaxValue;
        BaseObjectController[] toys = GameManager.Instance.Toys;
        for (int i = 0; i < toys.Length; i++)
        {
            float dis = Vector3.Distance(toys[i].transform.position, objectController_.transform.position);
            if ( dis < getToyDis && !toys[i].IsAlive)
            {
                if (dis < minDis)
                {
                    minDis = dis;
                    nestToy_ = toys[i];
                }
            }
        }

        if (nestToy_ != null)
        {
            nestToy_.setOutlineCol(Color.red);
            if (Input.GetKeyDown(getToyKey) && canCatchToy_)
            {
                nestToy_.setOutlineCol(Color.black);
                objectController_.deAliveLogic();
                ObjectController = nestToy_;
                camera_.GetComponent<CameraLogic>().player = objectController_.transform;
                camera_.GetComponent<CameraLogic>().transView(objectController_.transform,0.5f);
               
                //Destroy(this);
            }
        }
    }
    
    public virtual void beBeating(float othDamage)
    {
        //print("damage:"+othDamage);
        hp_ -= othDamage;
        print(gameObject.name+":HP="+hp_);
        GameManager.Instance.sendDamage(this, othDamage);
    }

    protected void dieLogic()
    {
        if (!isDie_)
        {
            objectController_.deathLogic();
            isDie_ = true;
        }
    }

    /*public void cloneFrom(BasePlayerController other)
    {
        hp_ = other.Hp;
        camera_ = other.camera_;
        print("cloneFrom:"+gameObject.name);
    }*/

    protected void moveLogic()
    {
        if (!canMove_)
        {
            return;
        }
        if (!hadObjController)
        {
            return;
        }
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
