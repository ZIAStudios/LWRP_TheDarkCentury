using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    public enum TroopType
    {
        Caballero,
        Enano
    }

    [System.Serializable]
    public class Pool           //Todos los pools que se vayan a hacer
    {
        public string name;
        public GameObject objectToPool;
        public int amountToPool;
    }

    #region Singletone - Can access the script without getting a component value
    public static UnitsManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public List<GameObject> pooledObjects; //La lista de los pools que se vayan a hacer

    public List<GameObject> activePooledObjects = new List<GameObject>(); //lista de los pools activos en la escena

    public List<GameObject> enemyActive = new List<GameObject>(); //list of enemies active in scene
    public List<GameObject> melee_Units = new List<GameObject>(); //list of melee units active in scene 
    public List<GameObject> spear_Units = new List<GameObject>(); //list of spear units active in scene 
    public List<GameObject> range_Units = new List<GameObject>(); //list of range units active in scene 

    int counter = 6;

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (Pool pool in pools)
        {
            for (int i = 0; i < pool.amountToPool; i++)
            {
                GameObject obj = Instantiate(pool.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }

    }
    //condition to create each list
    bool meleeToAdd = true;
    bool spearToAdd = true;
    bool rangeToAdd = true;

    private void Update()
    {
        //Add the active pooled objects to each list of objects (melee, range, spear)
        for (int i = 0; i < activePooledObjects.Count; i++)
        {   
            #region Lists of all the active units
            if (activePooledObjects[i].tag == "Player_Unit")
            {
                for (int j = 0; j < melee_Units.Count; j++)
                {
                    if (activePooledObjects[i] == melee_Units[j])
                    {
                        meleeToAdd = false;
                        break;

                    }
                    else
                    {
                        meleeToAdd = true;

                    }
                }
                if (meleeToAdd)
                {
                    melee_Units.Add(activePooledObjects[i]);
                }
            }

            if (activePooledObjects[i].tag == "Player_Spear")
            {
                for (int k = 0; k < spear_Units.Count; k++)
                {
                    if (activePooledObjects[i] == spear_Units[k])
                    {
                        spearToAdd = false;
                        break;

                    }
                    else
                    {
                        spearToAdd = true;

                    }
                }
                if (spearToAdd)
                {
                    spear_Units.Add(activePooledObjects[i]);
                }
            }

            if (activePooledObjects[i].tag == "Player_Range")
            {
                for (int l = 0; l < range_Units.Count; l++)
                {
                    if (activePooledObjects[i] == range_Units[l])
                    {
                        rangeToAdd = false;
                        break;

                    }
                    else
                    {
                        rangeToAdd = true;

                    }

                }
                if (rangeToAdd)
                {
                    range_Units.Add(activePooledObjects[i]);
                }
            }
            #endregion
        }

        UnitClassSelection();
    }

    #region UnitClassSelection() - Selecting all units active of 1 type of class

    void UnitClassSelection()
    {
        //Por cada gameobject (item) dentro de la Lista "X"
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (GameObject item in melee_Units)
            {   //Accedes al componente del hijo de la posición 6 (empezando desde 0)
                item.transform.GetChild(6).GetComponent<Selectable>().isSelected = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (GameObject item in spear_Units)
            {
                item.transform.GetChild(6).GetComponent<Selectable>().isSelected = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (GameObject item in range_Units)
            {
                item.transform.GetChild(6).GetComponent<Selectable>().isSelected = true;
            }
        }
    }
    #endregion


    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];

            }
        }
        return null;

    }

}
