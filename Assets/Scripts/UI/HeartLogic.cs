using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartLogic : MonoBehaviour
{
    private Animator animator_;
    // Start is called before the first frame update
    void Start()
    {
        animator_ = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void heartAwake()
    {
        //animator_.SetTrigger("Awake");
    }
    
    public void hreatBreak()
    {
        animator_.SetTrigger("Break");
        Invoke("selfDestory", 1f);
    }

    private void selfDestory()
    {
        gameObject.SetActive(false);
    }

    public void heartWarn(bool isWarn)
    {
        animator_.SetBool("Warn",isWarn);
    }
    
}
