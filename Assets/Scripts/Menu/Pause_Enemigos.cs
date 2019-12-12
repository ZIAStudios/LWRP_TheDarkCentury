using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Pause_Enemigos : MonoBehaviour
{
    
    public bool pausado;

    public GameObject panel_enemigos;

    public GameObject texto1;
    public GameObject texto2;
    
    void Start()
    {
   
        texto2.SetActive(false);
        texto1.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.timeScale);

    }

    public void Resume()
    {
        panel_enemigos.SetActive(false);
        Time.timeScale = 1f;
        pausado = false;
    }

    void Pause()
    {
        panel_enemigos.SetActive(true);
        Time.timeScale = 0f;
        pausado = true;
    }

    public void Volver()
    {
        panel_enemigos.SetActive(false);
        Time.timeScale = 1f;
        pausado = false;
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(1);
    }

    public void Siguiente_1()
    {
        texto2.SetActive(true);
        texto1.SetActive(false);   

        Debug.Log("Pulsando el botón 1");
    }

    public void Siguiente_2()
    {
        panel_enemigos.SetActive(false);
        Time.timeScale = 1f;
        pausado = false;
    }

}
