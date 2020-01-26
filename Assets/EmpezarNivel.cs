using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpezarNivel : MonoBehaviour
{
    public PruebaCámara camaraSequencia;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.name == "General")
        {

            camaraSequencia.cameraNormal.SetActive(false);
            camaraSequencia.cameraSequencia.SetActive(true);
            
           
            Destroy(gameObject);
        }
    }
}
