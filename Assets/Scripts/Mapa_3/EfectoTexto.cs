using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EfectoTexto : MonoBehaviour
{

    public float delay;

    public string textoEntero;

    private string textoActual = "";

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void MostrarTexto ()
    {

        StartCoroutine(Texto());
    }

    IEnumerator Texto ()
    {
        for (int i = 0; i < textoEntero.Length; i++)
        {
            textoActual = textoEntero.Substring(0, i);
            this.GetComponent<Text>().text = textoActual;
            yield return new WaitForSeconds(delay);
        }
    }
}
