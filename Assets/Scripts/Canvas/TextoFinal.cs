using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextoFinal : MonoBehaviour
{
    public GameObject general;
    public GameObject canvasPergamino;
    public Pergamino pergamino;
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.name == "General")
        {
            canvasPergamino.SetActive(true);
            
            pergamino.texto5.SetActive(true);
            Time.timeScale = 0f;
            FindObjectOfType<AudioManager>().Stop("AudioCinematica");
            FindObjectOfType<AudioManager>().Play("Mapa1Texto5");
            Destroy(gameObject);
        }
    }
}
