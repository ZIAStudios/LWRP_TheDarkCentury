﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject MenuPrincipal;
    public GameObject MenuInstrucciones;
    public GameObject soldados;

    void Start()
    {
        MenuInstrucciones.SetActive(false);
    }

    // Update is called once per frame
    public void Play()
    {
        SceneManager.LoadScene("Diseño_1");
    }

    public void Instrucciones ()
    {
        SceneManager.LoadScene("Mapa_2");
    }

    public void Back ()
    {
        MenuInstrucciones.SetActive(false);
        MenuPrincipal.SetActive(true);
        soldados.SetActive(true);
    }

    public void Exit ()
    {
        Application.Quit();
    }

}
