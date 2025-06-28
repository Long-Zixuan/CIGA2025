using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Ruler;

public class PencilController : BaseObjectController
{
    private RulerAttackColLogic rulerAttackColLogic_;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rulerAttackColLogic_ = attackCol.gameObject.GetComponent<RulerAttackColLogic>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    
    public override void attack()
    {
        base.attack();
        StartCoroutine(attackLogic());
    }

    IEnumerator attackLogic()
    {
        rulerAttackColLogic_.IsAttacking = true;
        yield return new WaitForSeconds(1.4f);
        rulerAttackColLogic_.IsAttacking = false;
        //print("攻击结束");
    }
}
