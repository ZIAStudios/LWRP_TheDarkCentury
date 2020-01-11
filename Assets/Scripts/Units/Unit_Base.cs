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
    [SerializeField] LayerMask floacklayer ;
    [SerializeField] float flockradius = 1;
    [SerializeField] float flockStrength = 1;
    
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


        if (Input.GetKey(KeyCode.Y))
        {
            ResetStats();
        }

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

		if (gameObject.tag == "P_Aldeano" || gameObject.tag == "E_Aldeano")
		{
			Animations();
		}
        Flock();
    }


    #region MoveToPoint() - Moves the object if it's selected
    public void MoveToPoint ()
    {
        if (selec.isSelected && Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //H
			if (Physics.Raycast(ray, out hit, 400, LayerMask.NameToLayer("Enemy")))
			{
				print("Hit Enemy");

				#region Sound - When click on a enemy with selected troops

				//Sonido al ir a por el enemigo
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
				#endregion

				#region If this.gameobject have STATE_MELEE and hit enemy
				//Si el enemigo tiene States_Melee
				if (gameObject.GetComponent<Combat_Melee>() != null )
				{
                    Combat_Melee myMelee = GetComponent<Combat_Melee>();
                    //if (!myMelee.clickOnEnemy)
                    myMelee.SetEnemy(hit.collider.transform.parent.gameObject);

                    myMelee.clickOnEnemy = true;
                    myMelee.state = Combat_Melee.State.clickEnemy;


                }
                //else -------------------------> PONER ESTO BIEN PARA EL ARQUERO
                //gameObject.GetComponent<Unit_Range>().enemyToChase = hit.collider.gameObject;
                #endregion
            }
			else if (Physics.Raycast(ray, out hit, 200, movementMask))
			{
                Combat_Melee myMelee = GetComponent<Combat_Melee>();

                #region Sound - When click on ground with selected troops
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
                #endregion

                #region If this.gameobject have COMBAT_MELEE and hit ground
                if (gameObject.GetComponent<Combat_Melee>() != null)
                { 
                    gameObject.GetComponent<Combat_Melee>().clickOnEnemy = false;
				
                    if (!myMelee.ignoreStates)
                    {
                        //Cambio al estado de forceMove, cuando esta atacando o alerta y lo quieres sacar y moverlo a otro lado
                        if (myMelee.state != Combat_Melee.State.normal && myMelee.state != Combat_Melee.State.forceMove)
                        {
                            myMelee.state = Combat_Melee.State.forceMove;
                            gameObject.GetComponent<Combat_Melee>().ignoreStates = true;
                        }
                    }
				}

				#endregion

				agent.SetDestination(hit.point);
			}

			//Movimiento que hace el villager específico
			if (gameObject.tag == "P_Aldeano")
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

                        }
						else
                        {
                            MoveAt(transform.position);
                        }
                        //lista para ver que está llendo a la posición (la casa)
                    }
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

    #region Flock() - Space between units
    void Flock()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, flockradius, floacklayer);
        Vector3 flockVector = Vector3.zero;
        int totalcolls = 0;
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].GetComponent<Unit_Base>() != null)
            {
                totalcolls++;
                flockVector += (transform.position - colls[i].transform.position).normalized * Mathf.Lerp(1, 0, Vector3.Distance(transform.position, colls[i].transform.position) / flockradius);
            }
        }
        if (totalcolls > 0)
        {
            flockVector /= totalcolls;
            //Debug.Log(flockVector);
        }
        agent.Move(flockVector * flockStrength);
    }
    #endregion

    #region Die()
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


        if (gameObject.tag == "P_Caballero")
        {
            objectPooler.melee_Units.Remove(gameObject);
        }
        if (gameObject.tag == "P_Arquero")
        {
            objectPooler.range_Units.Remove(gameObject);

        }
        if (gameObject.tag == "P_Lancero")
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
        gameObject.GetComponent<Positions>().Restart();
    }
    #endregion

    #region Animations() - Objects animations
    void Animations()
	{
		Vector3 curPos = transform.position;
		//if (curPos.x  >= lastPos.x - 0.0001 || curPos.x <= lastPos.x + 0.0001 || curPos.z >= lastPos.z - 0.0001 || curPos.z <= lastPos.z + 0.0001)
		if (Vector3.SqrMagnitude(curPos - lastPos) <= 0.01)
		//if (curPos == lastPos)
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
	#endregion



	private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, flockradius);
    }
}
