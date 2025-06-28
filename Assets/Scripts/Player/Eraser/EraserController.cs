using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserController : BaseObjectController
{
    public GameObject attackEffect;
    public float effectPosOffset = 0.5f;

    private List<GameObject> objsInAttckColl_ = new List<GameObject>();

    public List<GameObject> ObjsInAttckColl
    {
       get { return objsInAttckColl_; } 
    }

    public void addObjInAttckColl(GameObject obj)
    {
       objsInAttckColl_.Add(obj); 
    }

    public void removeObjInAttckColl(GameObject obj)
    {
       objsInAttckColl_.Remove(obj); 
    }
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void attack()
    {
        base.attack();
        StartCoroutine(attckLogic());
    }

    IEnumerator attckLogic()
    {
        yield return new WaitForSeconds(1.3f);
        GameObject effect1 = Instantiate(attackEffect, 
            transform.TransformPoint(new Vector3( effectPosOffset,0, 0)), 
            Quaternion.Euler(0,0,0),this.transform);
        GameObject effect2 =  Instantiate(attackEffect,
            transform.TransformPoint( new Vector3( effectPosOffset,0, 0)),
            Quaternion.Euler(0, 90, 0),this.transform);

        foreach (var obj in objsInAttckColl_)
        {
            Vector3 dir = (obj.transform.position - attackCol.transform.position).normalized;
            if (obj.GetComponent<BaseObjectController>() != null)
            {
                obj.GetComponent<Rigidbody>().AddForce(dir * attackForce, ForceMode.Impulse);
            }
            BasePlayerController playerController = obj.GetComponent<BasePlayerController>();
            if (playerController != null)
            {
                if (playerController != this.playerController_)
                {
                    playerController.beBeating(damage);
                }
            }
            
        }
        
    }
}
