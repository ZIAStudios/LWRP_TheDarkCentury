using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaCámara : MonoBehaviour
{

    public Animation anim;
    public Animator animator;

    public GameObject cameraSequencia;
    public GameObject cameraNormal;

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
}
