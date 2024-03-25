using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to handle quitting the game via a key
public class QuitApplication : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quitting");
        }
    }
}
