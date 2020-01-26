﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision : MonoBehaviour
{

    public GameObject canvasPergamino;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("P_Aldeano"))
        {
            Destroy(this.gameObject);
            canvasPergamino.SetActive(false);
            FindObjectOfType<AudioManager>().Stop("Mapa2Texto8");
            FindObjectOfType<AudioManager>().Play("Mapa2AuidioAnimo");
        }
    }
}