using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pergamino : MonoBehaviour
{
    public GameObject canvasPergamino;
    //public GameObject botónPergamino;
    public PruebaCámara camaraSequencia;

    public GameObject texto1;
    public GameObject texto2;
    public GameObject texto3;
    public GameObject texto4;
    public GameObject texto5;
    public GameObject texto6;
    public GameObject texto7;
    public GameObject texto8;
    public static bool Nivel1 = false;
    
    
    public bool paso3 = false;
    public bool paso5 = false;



    //public PruebaCámara scriptCámara;
    void Start()
    {
        camaraSequencia = GameObject.FindObjectOfType(typeof(PruebaCámara)) as PruebaCámara;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Siguiente()
    {
        FindObjectOfType<AudioManager>().Stop("Mapa2Texto1");
        FindObjectOfType<AudioManager>().Play("Mapa2Texto2");
        texto2.SetActive(true);
        texto1.SetActive(false);
    }

    public void SequenciaDos()
    {
        canvasPergamino.SetActive(false);
        camaraSequencia.SecuenciaDos();
        camaraSequencia.SecuenciaDosMapa2();
    }
    public void SequenciaTres()
    {
        FindObjectOfType<AudioManager>().Stop("Mapa2Texto3");
        FindObjectOfType<AudioManager>().Play("Mapa2Texto4");
        canvasPergamino.SetActive(false);
        paso3 = true;
        camaraSequencia.Secuencia3Mapa2();
    }
    public void Pasarde4a5()
    {
        FindObjectOfType<AudioManager>().Stop("Mapa2Texto4");
        FindObjectOfType<AudioManager>().Play("Mapa2Texto5");
        texto4.SetActive(false);
        texto5.SetActive(true);
    }
    public void SequenciaCuatro()
    {
        canvasPergamino.SetActive(false);
        paso5 = true;
        camaraSequencia.Secuencia4Mapa2();
    }
    public void Pasarde6a7()
    {
        FindObjectOfType<AudioManager>().Stop("Mapa2Texto6");
        FindObjectOfType<AudioManager>().Play("Mapa2Texto7");
        texto6.SetActive(false);
        texto7.SetActive(true);
    }
    public void EndCinema()
    {
        FindObjectOfType<AudioManager>().Stop("Mapa2Texto7");
        FindObjectOfType<AudioManager>().Play("Mapa2Texto8");
        texto7.SetActive(false);
        texto8.SetActive(true);
        camaraSequencia.Jugar();
    }
   
    public void Mapa1PostText1()
    {
        camaraSequencia.cameraNormal.SetActive(true);
        camaraSequencia.cameraSequencia.SetActive(false);
        texto2.SetActive(true);
        texto1.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Mapa1Texto2");
        FindObjectOfType<AudioManager>().Stop("Mapa1Texto1");
    }
    public void DesactivaTexto2()
    {
        texto2.SetActive(false);
        canvasPergamino.SetActive(false);
    }
   public void Desactivar3()
    {
        texto3.SetActive(false);
        canvasPergamino.SetActive(false);
    }

    public void VolverMenu()
    {
        SceneManager.LoadScene("SelectorNiveles");
        Nivel1 = true;
        
    }
    public void PasardeNivel()
    {
        SceneManager.LoadScene("Mapa_2");
    }
}