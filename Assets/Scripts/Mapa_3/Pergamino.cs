using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pergamino : MonoBehaviour
{
    public GameObject canvasPergamino;
    public GameObject botónPergamino;
    public PruebaCámara camaraSequencia;

    //public PruebaCámara scriptCámara;
    void Start()
    {
        camaraSequencia = GameObject.FindObjectOfType(typeof(PruebaCámara)) as PruebaCámara;
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void Siguiente ()
    {
        canvasPergamino.SetActive(false);

        camaraSequencia.SecuenciaDos();
    }
}
