using CartoonFX;
using DG.Tweening;
using UnityEngine;

/// <summary>

/// �����Ƿ������߼�������λ��

/// ��ⲻ�����Ͱ�����ƶ��������ߵ���ײ���ǰ��

/// </summary>

public class CameraLogic : MonoBehaviour

{

    public Transform player = null;            //��ɫͷ�������ÿ����壩λ����Ϣ

    private Vector3 tagetPostion;       //��������Ŀ���

    private Vector3 ve3;                //ƽ�������ref����

    Quaternion angel;                   //�������Ŀ�����תֵ

    public float speed;                 //����ƶ��ٶ�

    public float upFloat;               //Y����������

    public float backFloat;             //Z�������ǵľ���

    public float yOffect = 0;

    public string[] unColiTags;
    
    private Vector3 followPos;

    private bool follow_ = true;

    public bool Follow
    {
        set { follow_ = value; } 
    }
    
    void Start()
    {
        followPos = player.position + new Vector3(0, yOffect, 0);
    }
    
    void Update()
    {
        if (follow_)
        {
            followLogic();
        }

    }

    public void transView(Transform tagetTransform, float transTime)
    {
        follow_ = false;
        Vector3 followPos = tagetTransform.position + new Vector3(0, yOffect, 0);
        Vector3 tagetPos = followPos + player.up * upFloat - 
                   player.gameObject.GetComponent<BaseObjectController>().getForword() * backFloat;
        transform.DOMove(tagetPos,transTime,false);
        Quaternion tagetRotate = Quaternion.LookRotation( followPos - tagetPos);
        Vector3 tagetRotateEuler = tagetRotate.eulerAngles;
        transform.DORotate(tagetRotateEuler,transTime,RotateMode.Fast);
        Invoke("setFollowTrue", transTime);
    }

    void setFollowTrue()
    {
        follow_ = true;
    }

    void followLogic()
    {
        followPos = player.position + new Vector3(0, yOffect, 0);
      
        //��¼�����ʼλ��

        tagetPostion = followPos + player.up * upFloat - 
                       player.gameObject.GetComponent<BaseObjectController>().getForword() * backFloat;

        //[size = 12.6667px]//ˢ�����Ŀ��������

        tagetPostion = Function(tagetPostion);

        //���ǵ��ƶ��Ϳ���

        transform.position = Vector3.SmoothDamp(transform.position, tagetPostion, ref ve3, 0);

        angel = Quaternion.LookRotation(followPos - tagetPostion);

        transform.rotation = Quaternion.Slerp(transform.rotation, angel, speed);
    }

    /// <summary>

    /// ���߼�⣬����������Ƿ����������

    /// </summary>

    /// <param name="v3">�����������߷���ķ���</param>

    /// <returns>�Ƿ��⵽</returns>

    Vector3 Function(Vector3 v3)

    {

        RaycastHit hit;

        if (Physics.Raycast(player.position, v3 - player.position, out hit, 5.0f))

        {

            if (hit.collider.tag != "MainCamera" && isColi(hit.collider.tag))
            {

                v3 = hit.point + transform.forward * 0.5f;

            }

        }

        return v3;

    }


    bool isColi(string tag)
    {
        foreach(var t in unColiTags)
        {
            if( t == tag)
            {
                return false;
            }
        }
        return true;
    }


}