using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasEnemigos : MonoBehaviour
{
    public GameObject general;
    public GameObject panel_enemigos;
    public Pergamino pergamino;
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
            pergamino.texto3.SetActive(true);
            FindObjectOfType<AudioManager>().Stop("Mapa1Texto2");
            FindObjectOfType<AudioManager>().Play("Mapa1Texto3");


            Destroy(gameObject);
        }
    }
}
