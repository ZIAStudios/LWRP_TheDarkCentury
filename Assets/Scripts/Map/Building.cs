using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.AI;

public class Building : MonoBehaviour
{
    UnitsManager objectPooler;      //referencia al pool y al manager de tropas
	int layerToSetRally = 9;
	[TagSelector]
    public string playerObjectPoolTag = "";   //tag tropa aliada que crear (EDITOR)
	[TagSelector]
	public string aiObjectPoolTag = "";          //tag tropa AI que crear (EDITOR)
    [Space]
    Transform spawnPoint = null;     //sitio donde hacer spawn (EDITOR)
    BoxCollider spawnArea;
    [SerializeField] float cooldownToSpawn = 60;        //timepo entre spawns (EDITOR)
    [SerializeField] float currentTime = 60;
	Vector3 defaultSpawnAreaSpace;
    [SerializeField] bool defaultArea = false;
	[SerializeField] bool moveRallyPoint = false;


	[Space]
    public Zone.STATE buildingState;                    //Estado de la zona
    Zone.STATE lastBuildingState;                       //Guarda el último estado para reseter el timer de spawn
    [Space]
    [SerializeField] int maxTroops = 5;                 //Número de tropas máximo que spawnear en el edificio (EDITOR)
    [SerializeField] int enemyMaxTroops = 5;                 
    [SerializeField] int troopsToAddIfUpgrade = 1;
    int troopsExtra = 0;
    int basemaxTroops;
 
 [Space]
    public List<GameObject> enemyList = new List<GameObject>();     //lista tropas AI creadas en el edificio
    public List<GameObject> playerList = new List<GameObject>();    //lista tropas player creadas en el edificio
    
 [Space]
    public Slider buildingSlider;   //UI stuff (EDITOR)
    public TextMeshProUGUI playerCount;        //UI stuff (EDITOR)
    public TextMeshProUGUI enemyCount;         //UI stuff (EDITOR)
    
 [Space]
    public bool activeArmor = false;
    [SerializeField] int armorPlus = 30;
    public bool activeDamage = false;
    [SerializeField] int damagePlus = 30;

    public bool activeSlot = false;
    
 [Space]
    [SerializeField] bool spawnDirectly = false;
    int instantUnits = 0;
    [SerializeField] uint startUnits = 0;
    [SerializeField] bool baseActive = false; //si activas las mejoras de los sub edificios o no

	void Start()
    {
		objectPooler = UnitsManager.Instance;

		spawnArea = gameObject.transform.GetChild(1).gameObject.GetComponent<BoxCollider>();
        spawnPoint = transform.GetChild(0).transform;

        basemaxTroops = maxTroops;
        troopsExtra = maxTroops + troopsToAddIfUpgrade;

        currentTime = cooldownToSpawn;

		defaultSpawnAreaSpace = spawnArea.transform.position;
    }

    void Update()
    {
		RallyPoint();

		//UIItems();
        UpgradeSlot();


        //timer resetea cada vez que cambia de dueño la zona
        if (lastBuildingState != buildingState)
        {
            currentTime = cooldownToSpawn;
        }

		//el timer se ejecuta si la bandera no está en modo neutral y si las lista de los objectos no ha llegado a su máximo
		if (buildingState == Zone.STATE.Player)
		{
			if (playerList.Count < maxTroops)
			{
				currentTime -= Time.deltaTime;
			}
		}
		if (buildingState == Zone.STATE.AI)
		{
			if (enemyList.Count < enemyMaxTroops)
            {
                currentTime -= Time.deltaTime;
            }
        }

        //spawns dependiendo de quien controla la zona
        PlayerSpawn();
        AISpawn();

        if (spawnDirectly) //Spawn de unidades inicial, sin tener ue esperar X tiempo a cada reaparición de unidades
        {
            #region Spawn when game starts
            GameObject directSpawnObject = objectPooler.GetPooledObject(playerObjectPoolTag);
            directSpawnObject.GetComponent<Unit_Base>().buildingTag = gameObject.tag;

            if (directSpawnObject != null)
            {
                //listas a añadir el GameObject creado
                playerList.Add(directSpawnObject);
                objectPooler.activePooledObjects.Add(directSpawnObject);
                //sitio en el que aparecer
                directSpawnObject.transform.position = spawnPoint.position;
                directSpawnObject.transform.rotation = spawnPoint.rotation;

                //que se vea el objecto
                directSpawnObject.SetActive(true);
                //Donde se mueve al aparecer
                directSpawnObject.GetComponent<Unit_Base>().MoveAt(GetRandomPosition());

            }
            #endregion

            instantUnits++;
            print("spawns");
        }
        if (instantUnits >= startUnits)
            spawnDirectly = false;

;

        //timer dueño (realción con la primera condición)
        lastBuildingState = buildingState;
    }

    #region PlayerSpawn() - Spawn units depending on the zone state
    void PlayerSpawn()
    {
        if (buildingState == Zone.STATE.Player)
        {
            if (playerList.Count < maxTroops)
            {
                if (currentTime <= 0)
                {
                    GameObject PlayerObject = objectPooler.GetPooledObject(playerObjectPoolTag);
					PlayerObject.GetComponent<Unit_Base>().buildingTag = gameObject.tag;

					if (PlayerObject != null)
                    {
                        //listas a añadir el GameObject creado
                        playerList.Add(PlayerObject);
                        objectPooler.activePooledObjects.Add(PlayerObject);
                        //sitio en el que aparecer
                        PlayerObject.transform.position = spawnPoint.position;
                        PlayerObject.transform.rotation = spawnPoint.rotation;

                        //-----------------------------Condiciones cuando cambian los stats de la unidad y cuidado con la diversificación de las clases
                        if (!baseActive) //referencia a la base aliada, que no tiene subedificios
                        {
                            if (activeDamage) //CUANDO SE ACTIVA EL BOOST
                            {
                                PlayerObject.GetComponent<Unit_Base>().damage = damagePlus;

                                if (PlayerObject.tag == "Player_Unit") // SI AL FINAL CAMBIAS LO DEL MODEL HAY QUE CAMBIAR TODO ESTO Y PONER UN CHILD MÁS
                                {
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                                }
                                if (PlayerObject.tag == "Player_Range")
                                {
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);

                                }
                                if (PlayerObject.tag == "Player_Spear")
                                {
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);

                                }

                            }
                            else //CUANDO SE DESACTIVA EL OBJETO
                            {
                                PlayerObject.GetComponent<Unit_Base>().damage = PlayerObject.GetComponent<Unit_Base>().startDamage;

                                if (PlayerObject.tag == "Player_Unit")
                                {
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
                                }
                                if (PlayerObject.tag == "Player_Range")
                                {
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
                                }
                                if (PlayerObject.tag == "Player_Spear")
                                {
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                                }
                            }
                            if (activeArmor) //CUANDO SE ACTIVA EL BOOST
                            {
                                PlayerObject.GetComponent<Unit_Base>().armor = armorPlus;

                                if (PlayerObject.tag == "Player_Unit")
                                {
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);

                                }
                                if (PlayerObject.tag == "Player_Range")
                                {
                                    PlayerObject.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                                }
                                if (PlayerObject.tag == "Player_Spear")
                                {

                                    PlayerObject.transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
                                }

                            }
                            else //CUANDO SE DESACTIVA EL OBJETO
                            {
                                PlayerObject.GetComponent<Unit_Base>().armor = PlayerObject.GetComponent<Unit_Base>().startArmor;

                                if (PlayerObject.tag == "Player_Unit")
                                {
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
                                    PlayerObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                                }
                                if (PlayerObject.tag == "Player_Range")
                                {
                                    PlayerObject.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                                }
                                if (PlayerObject.tag == "Player_Spear")
                                {
                                    PlayerObject.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
                                }

                            }
                        }
                        //que se vea el objecto
                        PlayerObject.SetActive(true);
                        //Donde se mueve al aparecer
                        PlayerObject.GetComponent<Unit_Base>().MoveAt(GetRandomPosition());

                        currentTime = cooldownToSpawn;
                    }
                }
            }
        }
    }
	#endregion

	#region AISpawn()
	void AISpawn()
    {
        if (buildingState == Zone.STATE.AI)
        {
            if (enemyList.Count < enemyMaxTroops)
            {
                if (currentTime <= 0)
                {
                    GameObject AIObject = objectPooler.GetPooledObject(aiObjectPoolTag);
                    AIObject.GetComponent<Unit_Base>().buildingTag = gameObject.tag.ToString();

                    if (AIObject != null)
                    {
                        //lista a añadir el GAmeObject creado
                        enemyList.Add(AIObject);
                        objectPooler.activePooledObjects.Add(AIObject);
                        //sitio en el que aparecer
                        AIObject.transform.position = spawnPoint.position;
                        AIObject.transform.rotation = spawnPoint.rotation;                    
                        //que se active el objecto
                        AIObject.SetActive(true);
                        //Donde se mueve al aparecer
                        AIObject.GetComponent<Unit_Base>().MoveAt(GetRandomPosition());

                        currentTime = cooldownToSpawn;
                    }
                }
            }
        }
    }
    #endregion

    #region GetRandomPosition() - Postion where the unit is going to move after spawn
    Vector3 GetRandomPosition()
    {
        Vector3 spot = new Vector3(
            Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), 0f,
            Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
        );
        return spot;
    }
	#endregion

	#region UIItems() - Visual feedback from the building
	void UIItems()
    {
        buildingSlider.value = currentTime / cooldownToSpawn;
        playerCount.text = playerList.Count + " / " + maxTroops;
       enemyCount.text = enemyList.Count + " / " + enemyMaxTroops;
    }
    #endregion

    #region UpgradeSlot() - Add an slot of unit
    void UpgradeSlot()
    {
        if (activeSlot)
        {
            maxTroops = troopsExtra;
        }
        if (!activeSlot)
        {
            maxTroops = basemaxTroops;
        }
    }
	#endregion

	#region RallyPoint() - 

	void RallyPoint()
	{

        if (defaultArea)
        {
            spawnArea.transform.position = defaultSpawnAreaSpace;

            defaultArea = false;
        }

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 400))
		{

			//Deselección y selección de la condición para poder mover el spawn area
			if (Input.GetMouseButtonDown(0) && moveRallyPoint)
			{

				if (hit.collider.gameObject == this.gameObject && moveRallyPoint)
				{
					moveRallyPoint = false;
				}
				if (hit.collider.gameObject != this.gameObject)
				{
					moveRallyPoint = false;
				}
			}
			else if (Input.GetMouseButtonDown(0) && !moveRallyPoint)
			{

				if (hit.collider.gameObject == this.gameObject)
				{
					moveRallyPoint = true;
				}
			}

            //Movimiento del spawn area a donde se selecciona
			if (hit.collider.gameObject.layer == 9)				
			{
				if (Input.GetMouseButtonDown(1) && moveRallyPoint)
				{
                    spawnArea.transform.position = hit.point;
                    moveRallyPoint = false;
				}
			}
		}

	}



	#endregion

}
