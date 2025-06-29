using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUILogic : MonoBehaviour
{
    public HeartLogic[] hearts;
    private int heartsCount = 0;

    public GameObject winText;
    public GameObject loseText;
    // Start is called before the first frame update
    void Start()
    {
        heartsCount = hearts.Length ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void heartMinus(int minsVal)
    {
        //print("mins:"+minsVal);
        int beginDir = Math.Max(0, heartsCount - minsVal);
       // int ofs = beginDir - heartsCount + minsVal;
        try
        {
            for (int i = heartsCount - minsVal; i < heartsCount; i++)
            {
                //print( i+":"+hearts[i]);
                hearts[i].hreatBreak();
            }

            heartsCount -= minsVal;
        }
        catch (Exception e)
        {
            print(e);
        }
        
    }

    public void win()
    {
        winText.SetActive( true);
    }

    public void lose()
    {
        loseText.SetActive( true);
    }
    
}
