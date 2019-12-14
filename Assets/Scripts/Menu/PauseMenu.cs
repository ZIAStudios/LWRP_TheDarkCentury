using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPausa;
    public bool pausado;
    
   

    public GameObject texto1;
    public GameObject texto2;
    
    void Start()
    {
        /*pausado = true;
        Time.timeScale = 0f;

       menuPausa.SetActive(true);
        texto2.SetActive(false);
        texto1.SetActive(true);*/
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.timeScale);

  

    }

    public void Resume()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        pausado = false;
    }

    void Pause()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;
        pausado = true;
    }

    public void Volver()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        pausado = false;
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(1);
    }

    public void Siguiente_1()
    {
        texto1.SetActive(false);
        texto2.SetActive(true);

        Debug.Log("Hola");
    }

    public void Siguiente_2()
    {
        menuPausa.SetActive(false);
       
    }
    public void PasaEscena()
    {
        SceneManager.LoadScene("Menu");
    }

}
