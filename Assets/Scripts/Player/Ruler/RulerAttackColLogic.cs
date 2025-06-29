using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Ruler
{
    public class RulerAttackColLogic : MonoBehaviour
    {
        [SerializeField]
        private BaseObjectController rulerController_;//凑合一下
        public GameObject hitEffect;
        private bool isAttacking_;

        public bool IsAttacking
        {
            get { return isAttacking_; }
            set { isAttacking_ = value; }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (isAttacking_)
            {
                Vector3 hitPos = other.bounds.ClosestPoint(transform.position);
                Instantiate(hitEffect, hitPos, Quaternion.identity);
                Vector3 dir = (other.transform.position - rulerController_.transform.position).normalized;
                BaseObjectController baseObjectController = other.GetComponent<BaseObjectController>();
                if (baseObjectController != null)
                {
                    other.GetComponent<Rigidbody>().AddForce(dir * rulerController_.attackForce, ForceMode.Impulse);
                    if (baseObjectController.PlayerController != null)
                    {
                        baseObjectController.PlayerController.beBeating(rulerController_.damage);
                    }
                    
                }
                /*BasePlayerController playerController = other.GetComponent<BasePlayerController>();
                if (playerController != null)
                {
                    if (playerController != rulerController_.PlayerController)
                    {
                        playerController.beBeating(rulerController_.damage);
                    }
                }*/
            }
        }
    }
}
