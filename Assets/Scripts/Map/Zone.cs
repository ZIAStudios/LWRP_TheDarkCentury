using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    
    [SerializeField] float height = 100f;

    public LayerMask flagLayer;
    public LayerMask buildingLayer;
    public LayerMask subBuildingLayer;


    
    [SerializeField] Collider[] flagArray;
    [SerializeField] Collider[] buildingArray;
    [SerializeField] Collider[] subBuildingArray;

    [SerializeField] bool canChangeColorRoof = false;

    public enum STATE { Neutral, AI, Player }

    [SerializeField] STATE zoneState;

    STATE lastState;
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

            if (canChangeColorRoof)
            {
                if (col.transform.gameObject.GetComponent<Building>().buildingState == STATE.Player)
                {
                    col.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(true);
                    col.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(false);

                }
                if (col.transform.gameObject.GetComponent<Building>().buildingState == STATE.AI)
                {
                    col.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(true);
                    col.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(false);
                }
            }
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

            if (canChangeColorRoof)
            {
                if (col.transform.gameObject.GetComponent<Building_boost>().boostBuildingState == STATE.Player)
                {
                    col.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(true);
                    col.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(false);

                }
                if (col.transform.gameObject.GetComponent<Building_boost>().boostBuildingState == STATE.AI)
                {
                    col.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(true);
                    col.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(false);
                }
            }
        }

    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(transform.localScale.x, transform.localScale.y + height, transform.localScale.z));
    }

}
