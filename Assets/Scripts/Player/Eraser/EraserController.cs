using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EraserController : BaseObjectController
{
    public GameObject attackEffect;
    public float effectPosOffset = 0.5f;

    public float attackBkFloatOffect = 7f;
    public float attackYOffect = 5f;

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
        if (timer_ < attackSpeed)
        {
            return;
        }
        base.attack();
       
        StartCoroutine(attckLogic());
    }

    IEnumerator attckLogic()
    {
        playerController_.CanCatchToy = false;
        GameObject cameraObj = playerController_.camera_.gameObject;
        
        CameraLogic cameraLogic = playerController_.camera_.GetComponent<CameraLogic>();
        float beginBkFloat = cameraLogic.backFloat;
        float beginYOffect = cameraLogic.yOffect;
        DOTween.To(
                () => cameraLogic.backFloat,
                x => cameraLogic.backFloat = x,
                cameraLogic.backFloat + attackBkFloatOffect,
                1.3f
            ).SetEase(ease: Ease.OutQuad) //缓动类型
            .SetUpdate(true); 
        DOTween.To(
                () => cameraLogic.yOffect,
                x => cameraLogic.yOffect = x,
                cameraLogic.yOffect + attackYOffect,
                1.3f
            ).SetEase(ease: Ease.OutQuad) //缓动类型
            .SetUpdate(true); 
        playerController_.CanMove = false;
        yield return new WaitForSeconds(1.3f);
        cameraObj.transform.DOShakePosition(1, new Vector3(3, 3, 0));
        GameObject effect1 = Instantiate(attackEffect, 
            transform.TransformPoint(new Vector3( effectPosOffset,0, 0)), 
            Quaternion.Euler(0,0,0),this.transform);
        GameObject effect2 =  Instantiate(attackEffect,
            transform.TransformPoint( new Vector3( effectPosOffset,0, 0)),
            Quaternion.Euler(0, 90, 0),this.transform);

        foreach (var obj in objsInAttckColl_)
        {
            Vector3 dir = (obj.transform.position - attackCol.transform.position).normalized;
            BaseObjectController objController = obj.GetComponent<BaseObjectController>();
            if (objController != null)
            {
                obj.GetComponent<Rigidbody>().AddForce(dir * attackForce, ForceMode.Impulse);
                if (objController.PlayerController != null)
                {
                    objController.PlayerController.beBeating(damage);
                }
            }
        }
        //yield return new WaitForSeconds(0.1f);
        DOTween.To(
                () => cameraLogic.backFloat,
                x => cameraLogic.backFloat = x,
                beginBkFloat,
                0.2f
            ).SetEase(ease: Ease.OutQuad) //缓动类型
            .SetUpdate(true); 
        DOTween.To(
                () => cameraLogic.yOffect,
                x => cameraLogic.yOffect = x,
                beginYOffect,
                0.2f
            ).SetEase(ease: Ease.OutQuad) //缓动类型
            .SetUpdate(true); 
        yield return new WaitForSeconds(0.5f);
        playerController_.CanMove = true;
        playerController_.CanCatchToy = true;
    }
}
