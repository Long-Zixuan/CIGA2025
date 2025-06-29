using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    
    private static GameManager instance_s;
    public static GameManager Instance
    {
        get
        {
            if (instance_s == null)
            {
                instance_s = FindObjectOfType<GameManager>();
            }
            return instance_s;
        }
    }

    public BasePlayerController player1Controller;
    public BasePlayerController player2Controller;

    private BaseObjectController[] toys_;
    public BaseObjectController[] Toys
    {
        get
        {
            return toys_;
        }
    }
    private List<GameManagerListener> gameManagerListeners_ = new List<GameManagerListener>();

    private void Awake()
    {
        if (instance_s == null)
        {
            instance_s = this;
            toys_ = FindObjectsOfType<BaseObjectController>();
            print("toys_:"+toys_.Length);
            int index = UnityEngine.Random.Range(0, toys_.Length);
            if (player1Controller == null)
            {
                player1Controller = GameObject.Find("Player1Controller").gameObject.GetComponent<BasePlayerController>();
            }
            if (player2Controller == null)
            {
                player2Controller = GameObject.Find("Player2Controller").gameObject.GetComponent<BasePlayerController>();
            }
            player1Controller.ObjectController=toys_[index];
            player2Controller.ObjectController=toys_[(index + 1) % toys_.Length];
        }
        else
        {
            Debug.LogWarning("There is more than one instance of GameManager in the scene!");
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameRunningLogic();
    }

    void GameRunningLogic()
    {
        if (player1Controller.IsDie)
        {
            
        }
        if (player2Controller.IsDie)
        {
            
        }
    }

    public void addListener(GameManagerListener listener)
    {
        gameManagerListeners_.Add(listener);
    }

    public void removeListener(GameManagerListener listener)
    {
        gameManagerListeners_.Remove(listener);
    }
    
}
