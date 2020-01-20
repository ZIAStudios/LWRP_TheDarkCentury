using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pergamino : MonoBehaviour
{
    public GameObject canvasPergamino;
    //public GameObject botónPergamino;
    public PruebaCámara camaraSequencia;

    public GameObject texto1;
    public GameObject texto2;


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
        texto1.SetActive(false);
        texto2.SetActive(true);
    }

    public void SequenciaDos ()
    {
        canvasPergamino.SetActive(false);

        camaraSequencia.SecuenciaDos();
    }
}
