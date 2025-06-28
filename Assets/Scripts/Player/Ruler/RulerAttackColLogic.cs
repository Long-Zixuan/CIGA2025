using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Ruler
{
    public class RulerAttackColLogic : MonoBehaviour
    {
        [SerializeField]
        private RulerController rulerController_;
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
                if (other.GetComponent<BaseObjectController>() != null)
                {
                    other.GetComponent<Rigidbody>().AddForce(dir * rulerController_.attackForce, ForceMode.Impulse);
                }
                BasePlayerController playerController = other.GetComponent<BasePlayerController>();
                if (playerController != null)
                {
                    if (playerController != rulerController_.PlayerController)
                    {
                        playerController.beBeating(rulerController_.damage);
                    }
                }
            }
        }
    }
}
