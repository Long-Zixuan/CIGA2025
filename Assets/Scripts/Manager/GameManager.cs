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
    
    private List<GameManagerListener> gameManagerListeners_ = new List<GameManagerListener>();

    private void Awake()
    {
        if (instance_s == null)
        {
            instance_s = this;
        }
        else
        {
            Debug.LogWarning("There is more than one instance of GameManager in the scene!");
            Destroy(gameObject);
        }
    }
    
    public BaseObjectController[] playerControllers_;
    // Start is called before the first frame update
    void Start()
    {
        playerControllers_ = FindObjectsOfType<BaseObjectController>();
        UnityEngine.Random.Range(0, playerControllers_.Length);
        
    }

    // Update is called once per frame
    void Update()
    {
        GameRunningLogic();
    }

    void GameRunningLogic()
    {
        
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
