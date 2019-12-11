using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_boost : MonoBehaviour
{
    public GameObject building;
    Units_Selection selected;
    public GameObject particle;

    public Zone.STATE boostBuildingState;
    
    public enum Upgrade{ None, Damage, Armor, Slot }
    [Space]
    [SerializeField] Upgrade unitUpgrade;
    //[SerializeField] bool upgradeActivated = false;
    [Space]
    public uint villagersWorking = 0;
    [Range(1f, 2f)] public uint maxVillagersWorking;

    [HideInInspector] public Vector3 spawnPoint;

    private void Awake()
    {

        selected = Units_Selection.selectionInstance;
    }

    void Start()
    {
        particle= gameObject.transform.GetChild(1).gameObject;
        spawnPoint = gameObject.transform.GetChild(0).gameObject.transform.position; //punto donde se envian las unidades al seleccionar el edificio

    }
    public void Update()
    {
        //if(villagersWorking > 0)
        //{
        //
        //    particle.SetActive(true);
        //}
        //else particle.SetActive(false);



    }
    //Triggers from child
    public void EnterTrigger(Collider inside)
    {
        if (boostBuildingState == Zone.STATE.Player)
        {
            if (villagersWorking != maxVillagersWorking)
            {
                if (inside.tag == "P_Villager")
                {
                    villagersWorking++;
					FindObjectOfType<AudioManager>().Play("CampesinoTrabajando");
					switch (unitUpgrade)
                    {
                        case Upgrade.Slot:
                            building.GetComponent<Building>().activeSlot = true;
                            print("Slot");

                            break;
                        case Upgrade.Damage:
                            building.GetComponent<Building>().activeDamage = true;
                            print("Damage");

                            break;
                        case Upgrade.Armor:
                            building.GetComponent<Building>().activeArmor = true;
                            print("Armor");

                            break;

                    }
                }

            }
        }

    }
    public void ExitTrigger(Collider outside)
    {
        if (boostBuildingState == Zone.STATE.Player)
        {
            if (outside.tag == "P_Villager")
            {
                villagersWorking--;

				FindObjectOfType<AudioManager>().Stop("CampesinoTrabajando");

                switch (unitUpgrade)
                {
                    case Upgrade.Slot:
                        building.GetComponent<Building>().activeSlot = false;
                        print("Slot");

                        break;
                    case Upgrade.Damage:
                        building.GetComponent<Building>().activeDamage = false;
                        print("Damage");

                        break;
                    case Upgrade.Armor:
                        building.GetComponent<Building>().activeArmor = false;
                        print("Armor");

                        break;

                }
            }
        }
    }


    /*for (int i = 0; i < selected.GetSelected().Count; i++)
    {
        if (selected.GetSelected()[i].gameObject.transform.parent.gameObject.tag == "Villager")
        {

        }
    }*/
}

