using System.Collections;
using System.Collections.Generic;
using Mod;
using UnityEngine;

public class Main2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.OnMainStart();   
    }

    // Update is called once per frame
    void Update()
    {
        GameEvents.OnUpdateMain();
    }

    void FixedUpdate()
    {
        GameEvents.OnFixedUpdateMain();
    }

    void OnApplicationPause(bool pause)
    {    
        GameEvents.OnGamePause(pause);
    }

    void OnApplicationQuit()
    {
        GameEvents.OnGameClosing();
    }
}
