using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasEnemigos : MonoBehaviour
{
    public GameObject general;
    public GameObject panel_enemigos;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "General")
        {
            panel_enemigos.SetActive(true);
            Time.timeScale = 0f;

            Destroy(gameObject);
        }
    }
}
