using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaCámara : MonoBehaviour
{

    public Animation anim;
    public Animator animator;

    public GameObject cameraSequencia;
    public GameObject cameraNormal;
    public Pergamino pergamino;
    public GameObject canvasTexto;

    
    //public GameObject UI;
    void Start()
    {
        //StartCoroutine(Sequencia());
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Pergamino", canvasTexto);

        if (canvasTexto.activeInHierarchy == false)
        {
            Debug.Log("Canvas texto falso, Sequencia 2¿?");         
        }
        
    }

    /*IEnumerator Sequencia ()
    {
        yield return new WaitForSeconds(10);

        cameraSequencia.SetActive(false);
        cameraNormal.SetActive(true);
    }*/

    void TextoInterfaz ()
    {
        canvasTexto.SetActive(true);
        //UI.SetActive(true);
    }

    public void SecuenciaDos ()
    {
        if (canvasTexto.activeInHierarchy == false)
        {
            print("Canvas texto falso, Sequencia 2¿?");
            anim.Play("Sequencia_2");
        }
    }
    public void SecuenciaDosMapa2()
    {
        if (canvasTexto.activeInHierarchy == false)
        {
            print("Canvas texto falso, Sequencia 2¿?");
            anim.Play("Mapa2_Secuencia2");
        }
    }
    void Texto3Act()
    {
        canvasTexto.SetActive(true);
        pergamino.texto2.SetActive(false);
        pergamino.texto3.SetActive(true);
    }

    void Texto4Act()
    {
        canvasTexto.SetActive(true);
        pergamino.texto3.SetActive(false);
        pergamino.texto4.SetActive(true);
    }
    void Texto6Act()
    {
        canvasTexto.SetActive(true);
        pergamino.texto5.SetActive(false);
        pergamino.texto6.SetActive(true);
    }
    public void Secuencia3Mapa2()
    {
        if(pergamino.paso3 == true)
        {
            anim.Play("Mapa2_Secuencia3");
        }
    }

    public void Secuencia4Mapa2()
    {
        if (pergamino.paso5 == true)
        {
            anim.Play("Mapa2_Secuencia4");
        }
    }

    public void Jugar()
    {
        cameraNormal.SetActive(true);
        cameraSequencia.SetActive(false);
    }
}
