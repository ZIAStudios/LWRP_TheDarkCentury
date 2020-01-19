using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackPoint : MonoBehaviour
{
    public Collider[] enemyInPoint; 
    public float radius = 2f;
    public LayerMask layerToDetect; //----------------------> HACER REFERENCIA EN POSITIONS PARA EDICIÓN MÁS RÁPIDA

    public int index = 0; //SI VA MAL ES PORQUE HAY UQE EDITARLO PARA CADA PUNTO DE ATAQUE DE LA ESCENA

    private void Update()
    {   //Si detecta la "layerToDetect"
        enemyInPoint = Physics.OverlapSphere(transform.position, radius, layerToDetect); //Si detecta enemigo dentro del overlap, este se elimina de la lista
        //Checkear si hay algo

            if (enemyInPoint.Length == 1) //si hay un enemigo dentro se vacía el array
            {
				if (!enemyInPoint[0].gameObject.GetComponent<States_Melee>().ignoreStates)
				{
					gameObject.GetComponentInParent<Positions>().emptyPositions[index] = false;
					print(enemyInPoint[0].gameObject.name + " false");
				}

            }
            if (enemyInPoint.Length < 1) // si no hay un enemigo se rellena ese index del array
            {
                gameObject.GetComponentInParent<Positions>().emptyPositions[index] = true;
            }



    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
