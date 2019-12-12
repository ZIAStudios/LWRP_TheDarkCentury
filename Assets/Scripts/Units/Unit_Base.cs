using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit_Base : MonoBehaviour
{
	public Animator anim;
    UnitsManager objectPooler;
    Selectable selec;
    Building building;
    [HideInInspector] public NavMeshAgent agent;

    public LayerMask movementMask;
    public LayerMask subBuilding;

    [Space]

    [Header("Movement Stats")]
    public float speed = 6f;
    public float rotationSpeed = 500f;
    public float acceleration = 12f;
    [Header("Combat Stats")]
    public int maxHealth = 100;
    public int armor = 1;
    public int damage = 10;
    public float currentHealth;
    
    [HideInInspector] public float startSpeed;
    [HideInInspector] public float startRotationSpeed;
    [HideInInspector] public float startAcceleration;
    [HideInInspector] public int startMaxHealth;
    [HideInInspector] public int startArmor;
    [HideInInspector] public int startDamage;

    public string buildingTag;

    [HideInInspector] public Vector3 knockbackPoint;

    [SerializeField] bool alreadySpawns = false;

    //Timer para animaciones
    float time = 0.1f;
    float startTime = 0.1f;

	Vector3 lastPos;


	private void Awake()
    {
		anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();   
        selec = GetComponentInChildren<Selectable>();


        startSpeed = speed;
        startRotationSpeed = rotationSpeed;
        startAcceleration = acceleration;
        startMaxHealth = maxHealth;
        startArmor = armor;
        startDamage = damage;

    }

    void Start()
    {
        objectPooler = UnitsManager.Instance;           //referencia al pool

        currentHealth = maxHealth;
    }


    void Update()
    {
        if (buildingTag != "")
            building = GameObject.FindWithTag(buildingTag).GetComponent<Building>(); //GRAN GUARRADA Coge el edifio con el tag que se le haya puesto

        agent.speed = speed;
        agent.angularSpeed = rotationSpeed;
        agent.acceleration = acceleration;

        //Timer de animaciones
        if (anim.GetBool("TakeDamage") == true)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                time = startTime;
                anim.SetBool("TakeDamage", false);
            }
        }  


        MoveToPoint();
    }


    #region MoveToPoint() - Moves the object if it's selected
    public void MoveToPoint ()
    {
        if (selec.isSelected && Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 400, LayerMask.NameToLayer("Enemy")))
			{
				print("Hit Enemy");

				float hitSound = Random.Range(0, 25);
				if (hitSound <= 2)
				{
					FindObjectOfType<AudioManager>().Play("Yes");
				}
				if (hitSound > 2 && hitSound <= 4)
				{
					if (gameObject.tag != "P_Aldeano")
					{
						FindObjectOfType<AudioManager>().Play("MovSoldado");
					}
					else { FindObjectOfType<AudioManager>().Play("MovAldeano"); }
				}

				if (gameObject.GetComponent<States_Melee>() != null)
				{
					if (hit.collider.gameObject.GetComponentInParent<Positions>().positionsInUse < 6)
					{			
						//hardcodeas un ataque a un target , seteas cual es el best target y te mueves a él
						
						//gameObject.GetComponent<States_Melee>().bestTarget = hit.collider.transform.parent.gameObject;
						//MoveAt(hit.collider.gameObject.transform.position);
						gameObject.GetComponent<States_Melee>().SetEnemy(hit.collider.transform.parent.gameObject);
						print(hit.collider.transform.parent.gameObject.name);
					}
				}
				//else -------------------------> PONER ESTO BIEN PARA EL ARQUERO
				//gameObject.GetComponent<Unit_Range>().enemyToChase = hit.collider.gameObject;

			}
			else if (Physics.Raycast(ray, out hit, 200, movementMask))
			{
				print("Select and click");

				float hitSound = Random.Range(0, 25);
				if (hitSound <= 2)
				{
					FindObjectOfType<AudioManager>().Play("Yes");
				}
				if (hitSound > 2 && hitSound <= 4)
				{
					if (gameObject.tag != "P_Aldeano")
					{
						FindObjectOfType<AudioManager>().Play("MovSoldado");
					}
					else { FindObjectOfType<AudioManager>().Play("MovAldeano"); }
				}

				if (gameObject.GetComponent<States_Melee>() != null)
				{
					if (gameObject.GetComponent<States_Melee>().state != States_Melee.State.normal)
					{
						gameObject.GetComponent<States_Melee>().ignoreStates = true;
						gameObject.GetComponent<States_Melee>().state = States_Melee.State.normal;
					}

					if (gameObject.GetComponent<States_Melee>().bestTarget != null)
					{
						gameObject.GetComponent<States_Melee>().bestTarget = null;
					}
				}

				//ESTO CAMBIARLO CUANDO SE QUEDE EL CÓDIGO NUEVO
				if (gameObject.GetComponent<Unit_Melee>() != null)
				{
					if (gameObject.GetComponent<Unit_Melee>().enemyToChase != null)
					{
						gameObject.GetComponent<Unit_Melee>().enemyToChase = null;
					}
				}
				
				agent.SetDestination(hit.point);
			}

			//Movimiento que hace el villager específico
			if (gameObject.tag == "P_Villager")
            {
                if (Physics.Raycast(ray, out hit, 200, subBuilding))
                {
                    print("building boost hit");
                    if (hit.collider.GetComponent<Building_boost>().boostBuildingState == Zone.STATE.Player)
                    {
                        //-----------------------ESTO ESTA HARDCODEADO-> CAMBAIRLO
                        if(hit.collider.GetComponent<Building_boost>().villagersWorking < 1)
                        {
                            MoveAt(hit.collider.GetComponent<Building_boost>().spawnPoint);

                        }else
                        {
                            MoveAt(transform.position);
                        }
                        //lista para ver que está llendo a la posición (la casa)
                    }
                }

				if (agent.hasPath)
				{
					anim.SetBool("Move", true);
				}
				else
				{
					anim.SetBool("Move", false);

				}
			}
        }
    }
    #endregion

    #region MoveAt() - moves to the spot set
    public void MoveAt(Vector3 spot)
    {
        agent.SetDestination(spot);
    }
    #endregion

    #region TakeDamage (int damage) - When it takes damage
    public void TakeDamage(float damage)
    {
        damage = damage/armor;
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();

        }
        Debug.Log(gameObject.name + " takes " + damage + " damage.");


    }
    #endregion

    public void Die()
    {
        if (!alreadySpawns) //cuando muere si esta ya spawneado y no es clon de la base, no se remueve de la lista (porque no tiene)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Enemy")) //si el objeto es enemigo, se elimina de la lista enemigo
            {
                building.enemyList.Remove(gameObject); // el edificio al que pertenece la tropa (puesto con un tag, el edificio) y a la lista de ese edificio
            }
            if (gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                building.playerList.Remove(gameObject);
            }
        }

        objectPooler.activePooledObjects.Remove(gameObject);
        //para que no se quede con el target al que a perseguido antes de morir
        if (gameObject.GetComponent<Unit_Melee>() != null)
        {
            gameObject.GetComponent<Unit_Melee>().state = Unit_Melee.State.wait;
        }
        if (gameObject.GetComponent<Unit_Range>() != null)
            gameObject.GetComponent<Unit_Range>().state = Unit_Range.State.wait;


        if (gameObject.tag == "Player_Unit")
        {
            objectPooler.melee_Units.Remove(gameObject);
        }
        if (gameObject.tag == "Player_Range")
        {
            objectPooler.range_Units.Remove(gameObject);

        }
        if (gameObject.tag == "Player_Spear")
        {
            objectPooler.spear_Units.Remove(gameObject);

        }

		FindObjectOfType<AudioManager>().Play("Muerte");
		


		gameObject.SetActive(false);
        transform.position = new Vector3 (transform.position.x, -100, transform.position.z);
        //------------------------------------------------------------------> Implementar reseteo de stats al morir

        Debug.Log(gameObject.name + " died.");

        ResetStats();

    }

    public void ResetStats()
    {

        Start();
    }

	void Animations()
	{
		Vector3 curPos = transform.position;
		if (curPos == lastPos)
		{
			anim.SetBool("Move", false);
		}
		else
		{
			lastPos = curPos;
			anim.SetBool("Move", true);
			anim.SetBool("Attacking", false);

		}

	}

}
