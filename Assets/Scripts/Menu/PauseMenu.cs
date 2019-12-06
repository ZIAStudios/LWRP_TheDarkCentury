using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPausa;
    public bool pausado;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado)
            {
                Resume();
                
            }
            else
            {
                Pause();
               
            }
        }
    }

    void Resume()
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
}
