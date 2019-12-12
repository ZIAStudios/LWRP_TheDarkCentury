using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_boostPoint : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        gameObject.GetComponentInParent<Building_boost>().EnterTrigger(c);
    }
    void OnTriggerExit(Collider c)
    {
        gameObject.GetComponentInParent<Building_boost>().ExitTrigger(c);
    }
}

