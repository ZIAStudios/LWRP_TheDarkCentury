using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (Unit_Base))]
public class Unit_Range : MonoBehaviour
{
    //ARREGLAR EL COOLDOWNATTACK PROVISIONAL POR TIMER (siempre ejecutandose)

    //[HideInInspector] public int toolbarTab;
    //public string currentTab;

    NavMeshAgent agent;
    Selectable selec;
    UnitsManager objectPooler;
    Building building;
    Unit_Base unit;

    #region States Fields
    public bool wait;
    public bool alert;
    public bool combat;
    public bool death;
    [Space]
    [SerializeField] bool ignoreStates;
    #endregion

    #region Interaction Fields
    [SerializeField] LayerMask toChaseMask = 11;
    [Space]
    [SerializeField] float alertRadius = 30f;
    [SerializeField] float distanceToCombat = 15f;
	[SerializeField] float distancePlus = 20f;
	float combatDistance = 10f;

	public GameObject enemyToChase;
    [Space]

    [Header("What to Throw")]
    [SerializeField] string proyectilePoolTag = "Arrow";
    [SerializeField] Transform spawnProyectile;
    [Space]
    [SerializeField] float attackSpeed = 1f;
    float currentTime;
    [Space]
    [SerializeField] float proyectileHeight = 10f;
    float height;
    [SerializeField] float gravity = -10f;

    
    [HideInInspector]
    public enum State
    {
        wait,
        alert,
        combat,
        death
    }
    [HideInInspector] public State state;

    #endregion

    Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        selec = GetComponentInChildren<Selectable>();

        building = FindObjectOfType<Building>();
        unit = GetComponent<Unit_Base>();
    }

    private void Start()
    {
        objectPooler = UnitsManager.Instance;
        state = State.wait;

        anim = GetComponentInChildren<Animator>();

		combatDistance = distanceToCombat;
    }

    private void Update()
    {
        if (agent.hasPath && state != State.combat)
        {
            anim.SetBool("Move", true);
            anim.SetBool("Attacking", false);


        }
        else if (!agent.hasPath && state != State.combat)
        {
            anim.SetBool("Move", false);
            anim.SetBool("Attacking", false);

        }
        else if (state == State.combat)
        {
            anim.SetBool("Attacking", true);
            anim.SetBool("Move", false);

        }


        height = proyectileHeight + transform.position.y;

        ForceToMove();
        MoveStates();

        if (!ignoreStates)
        {
            StateOfAlert();
            StateOfCombat();
        }

        switch (state)
        {
            case State.wait:
                wait = true;
                alert = false;
                combat = false;
                break;
            case State.alert:
                wait = false;
                alert = true;
                combat = false;
                break;
            case State.combat:
                wait = false;
                alert = false;
                combat = true;
                break;


        }
        if (death)
        {
            state = State.death;
        }
    }

    #region ForceToMove() - The unit is force to move when state mode is alert or combat
    private void ForceToMove()
    {
        //Condiciones para que deje de atacar o estar alerta
        if (selec.isSelected && Input.GetMouseButtonDown(1))
        {


            if (state == State.combat || state == State.alert)
            {
                ignoreStates = true;
                //if (agent.hasPath)
                {
                    print("ignoring");
                    state = State.wait;


                }
            }
        }
        else if (!agent.hasPath)
        {
            ignoreStates = false;
        }

    }
    #endregion

    #region MoveState() - Movement depends on the state
    void MoveStates()
    {
        switch (state)
        {
            case State.wait:
                unit.MoveToPoint();

                break;
            case State.alert:
                MoveTo(enemyToChase.transform.position);
                currentTime = attackSpeed;       //sets the atack time

                break;
            case State.combat:
                MoveTo(transform.position);
                FaceTarget();

                break;
            case State.death:
                //unit.Die();
                enemyToChase = null;

                break;
        }
    }
    #endregion

    #region MoveTo()/ FaceTarget() - Where To Move / Face the enemy when combat state
    void MoveTo(Vector3 position) //cambiando al varias entre estado alerta y combate
    {
        if (position != null)
        {
            agent.SetDestination(position);
        }
    }

    void FaceTarget() 
    {
        if (enemyToChase != null)
        {
            Vector3 direction = (enemyToChase.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
    #endregion

    #region StateOfAlert() - OverlapSphere that detects enemyes and choose the closest one (GetClosestEnemy(List<transform>)
    // Controla cuándo tienen que estar en estado de alerta y cuando en estado de espera.
    void StateOfAlert()
    {
        // Se crea una lista de enemigos dentro del area de alerta
        List<Transform> potentialEnemiesToAttack = new List<Transform>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, alertRadius, toChaseMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            potentialEnemiesToAttack.Add(colliders[i].gameObject.transform);
        }

        // Se encuentra el enemigo más cercano y se va a por él
        if (state != State.combat)
        {
            currentTime = attackSpeed;

            if (potentialEnemiesToAttack.Count != 0) //programasiooo defensivaa
            {
                enemyToChase = GetClosestEnemy(potentialEnemiesToAttack).gameObject;
                state = State.alert;
            }
            else
            {
                enemyToChase = null;
                state = State.wait;
            }
        }
    }

    Transform GetClosestEnemy(List<Transform> enemies)
    {
        // Encuentra al enemigo más cercano al soldado
        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform potentialTarget in enemies)
        {
            float distance = Vector3.SqrMagnitude(potentialTarget.position - transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }
    #endregion

    #region StateOfCombat() - When enemy is closer than X distance, it attacks
    // Controla cuándo tiene que pasar al estado de combate
    void StateOfCombat()
    {

        // Cuando la distancia entre el enemigo y el soldado es menor a X, entran en combate.
        if (enemyToChase != null)
        {

            float distance = Vector3.Distance(enemyToChase.transform.position, transform.position);
            if (distance <= combatDistance)
            {
                state = State.combat;

				combatDistance = distancePlus;
                //currentTime -= Time.deltaTime; //timer each attack

				//EL ATAQUE VA POR ANIMACIÓN
                //if (currentTime <= 0)
                //{
                //    //LaunchObject();
                //    //----------------------------------------------------> Llamar al ataque pertinente
                //    currentTime = attackSpeed;
                //}

            }
            else
            {
                state = State.alert;
				combatDistance = distanceToCombat;
            }
        }
    }
    #endregion

    [HideInInspector] public GameObject obj;

    #region LaunchObject() - Launches object towards the enemy set
    private void LaunchObject()
    {
        obj = objectPooler.GetPooledObject(proyectilePoolTag);
        obj.transform.position = spawnProyectile.position;
        obj.transform.rotation = spawnProyectile.rotation;

        Physics.gravity = Vector3.up * gravity;
        obj.GetComponent<Rigidbody>().useGravity = true;

        //Calcula la velocidad a la que sale el proyectil
        float displacementY = (enemyToChase.transform.position.y - obj.transform.position.y);
        Vector3 displacementXZ = new Vector3(enemyToChase.transform.position.x - obj.transform.position.x, 0, enemyToChase.transform.position.z - obj.transform.position.z);
        
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity));

        Vector3 finalVelocity = velocityXZ + velocityY;
        obj.SetActive(true);
        
        obj.GetComponent<Rigidbody>().velocity = finalVelocity;
        obj.GetComponent<Range_Proyectile>().proyectileDamage = unit.damage;

        print("Shooted");

    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, combatDistance);

		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere(transform.position, distanceToCombat);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, distancePlus);
	}
}