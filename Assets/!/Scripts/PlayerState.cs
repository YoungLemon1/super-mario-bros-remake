using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerState Instance { get; private set; }
    public bool IsAlive { get; private set; } = true;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public void DisablePlayerInput()
    {
        IsAlive = false;
    }
    public void EnablePlayerInput()
    {
        IsAlive = true;
    }
    
}
