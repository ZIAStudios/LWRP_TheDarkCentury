using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicaMapa1 : MonoBehaviour
{
    public PruebaCámara camaraSequencia;
    public Animation anim;
    public Animator animator;
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
            camaraSequencia.cameraNormal.SetActive(false);
            camaraSequencia.cameraSequencia.SetActive(true);
            anim.Play("Mapa1Secuencia2");
            FindObjectOfType<AudioManager>().Stop("Mapa1Texto3");
            FindObjectOfType<AudioManager>().Play("AudioCinematica");


            Destroy(gameObject);
        }

        
    }
}