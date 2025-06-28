using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Eraser
{


    public class AttackColLogic : MonoBehaviour
    {
        [SerializeField] private EraserController eraserController_;

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
            // 将进入的物体添加到集合中
            if (!eraserController_.ObjsInAttckColl.Contains(other.gameObject))
            {
                eraserController_.addObjInAttckColl(other.gameObject);
            }
        }

        // 当有物体离开触发器时调用
        private void OnTriggerExit(Collider other)
        {
            // 如果物体存在于集合中，则将其移除
            if (eraserController_.ObjsInAttckColl.Contains(other.gameObject))
            {
                eraserController_.removeObjInAttckColl(other.gameObject);
            }
        }
    }
}
