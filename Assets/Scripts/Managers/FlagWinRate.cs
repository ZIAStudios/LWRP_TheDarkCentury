using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagWinRate : MonoBehaviour
{
    Pergamino texto;
    [SerializeField] float height = 50f;


    public LayerMask flagLayer;

    [SerializeField] Collider[] flagArray;



    void Update()
    {

        OverlapFlag();


    }

    void OverlapFlag()
    {

        flagArray = Physics.OverlapBox(gameObject.transform.position, new Vector3(transform.localScale.x, transform.localScale.y + height, transform.localScale.z) / 2, Quaternion.identity, flagLayer);

        for (int i = 0; i < flagArray.Length; i++)
        {
            if (flagArray[0].gameObject.GetComponent<Flag>().flagState == Zone.STATE.Player && flagArray[1].gameObject.GetComponent<Flag>().flagState == Zone.STATE.Player && flagArray[2].gameObject.GetComponent<Flag>().flagState == Zone.STATE.Player)
            {
                texto.canvasPergamino.SetActive(true);
                texto.texto9.SetActive(true);
                //PONER AQUI QUE SE QUIERA QUE PASE CUANDO CAPTURE LAS 3 BANDERAS
                print("esto esta");
            }

        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(transform.localScale.x, transform.localScale.y + height, transform.localScale.z));
    }
}
