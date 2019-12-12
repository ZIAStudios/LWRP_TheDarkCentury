using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    /*
     * NOTA MUY IMPORTANTE POR LA MAESTRA DEL MULTIVERSO
     * 
     * CAMBIAR ESTA MIERDA DE UN SOLO ARRAY, PORQUE DETECTA TODO MUY RÁPIDO Y ESTÁ MAL HECHO. (hay que poner un array para cada uno)
     * 
     * */
    [SerializeField] float height = 100f;

    public LayerMask flagLayer;
    public LayerMask buildingLayer;
    public LayerMask subBuildingLayer;


    
    [SerializeField] Collider[] flagArray;
    [SerializeField] Collider[] buildingArray;
    [SerializeField] Collider[] subBuildingArray;


    public enum STATE { Neutral, AI, Player }

    [SerializeField] STATE zoneState;


    // Update is called once per frame
    void LateUpdate()
    {

        OverlapFlag();
        OverlapBuilding();
        BoostBuilding();

    }

    #region OverlapFlag() - Detecta si hay una bandera en la zona
    void OverlapFlag()
    {

        flagArray = Physics.OverlapBox(gameObject.transform.position, new Vector3(transform.localScale.x, transform.localScale.y + height, transform.localScale.z) / 2, Quaternion.identity, flagLayer);

        if (flagArray.Length == 1)
        {
            zoneState = flagArray[0].transform.gameObject.GetComponent<Flag>().flagState;
        }
        else
        {
            Debug.LogError(gameObject.name + "There's more than 1 flag or there's no flag inside the zone. Number of flags: " + flagArray.Length);
        }

    }
    #endregion


    #region OverlapBuilding() - Detecta los edificios que hay en la zona
    void OverlapBuilding()
    {

        buildingArray = Physics.OverlapBox(gameObject.transform.position, new Vector3(transform.localScale.x, transform.localScale.y + height, transform.localScale.z) / 2, Quaternion.identity, buildingLayer);
        foreach (Collider col in buildingArray)
        {
            col.transform.gameObject.GetComponent<Building>().buildingState = zoneState;
        }

    }
    #endregion

    #region BoostBuilding() - Detecta los edificios que hay en la zona
    void BoostBuilding()
    {

        subBuildingArray = Physics.OverlapBox(gameObject.transform.position, new Vector3(transform.localScale.x, transform.localScale.y + height, transform.localScale.z) / 2, Quaternion.identity, subBuildingLayer);
        foreach (Collider col in subBuildingArray)
        {
            col.transform.gameObject.GetComponent<Building_boost>().boostBuildingState = zoneState;
        }

    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(transform.localScale.x, transform.localScale.y + height, transform.localScale.z));
    }

}
