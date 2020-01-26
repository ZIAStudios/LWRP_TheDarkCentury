using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nivel1()
    {
        SceneManager.LoadScene("Diseño_1");
    }

    public void Nivel2()
    {
        if( Pergamino.Nivel1 == true)
        {
            SceneManager.LoadScene("Mapa_2");
        }
    }
}
