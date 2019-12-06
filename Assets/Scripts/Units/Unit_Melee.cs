using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Unit_Base))]
public class Unit_Melee : MonoBehaviour
{
    //EL COOLDOWNATTACK PROVISIONAL POR TIMER ARREGLAR

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
    [SerializeField] float alertRadius = 6f;
    [SerializeField] float distanceToCombat = 1.5f;


    [Space]
    public GameObject enemyToChase;
    [SerializeField] float attackSpeed = 5f;
    float currentTime;
    [Space]
    [SerializeField] float knockbackForce = 10f;
    public bool _knock = false;

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
    }

    private void Update()
    {

        if (state != State.combat)
        {
            if (agent.hasPath)
            {

                anim.SetBool("Move", true);
                anim.SetBool("Attacking", false);

            }
            if (!agent.hasPath)
            {

                anim.SetBool("Move", false);
                anim.SetBool("Attacking", false);
            }
        }
        if (state == State.combat)
        {

            anim.SetBool("Attacking", true);
            anim.SetBool("Move", false);

        }

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


                break;
            case State.combat:
                MoveTo(transform.position);
                FaceTarget();
                break;
            case State.death:
                enemyToChase = null;
                Debug.Log("Muere");

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
            if (distance <= distanceToCombat)
            {
                _knock = true;
                state = State.combat;
                currentTime -= Time.deltaTime; //timer each attack


                if (currentTime <= 0)
                {
                    HitEnemy();
                    //----------------------------------------------------> Llamar al ataque pertinente
                    //enemyToChase.GetComponent<Rigidbody>().AddExplosionForce(knockbackForce, gameObject.transform.position, distanceToCombat + 4f, 3f, ForceMode.Impulse);


                    currentTime = attackSpeed;
                }

            }
            else
            {
                state = State.alert;
                _knock = false;
            }
        }
    }
    #endregion

    void HitEnemy()
    {
        print("hit");
        var enemy = enemyToChase.GetComponent<Unit_Base>();

        enemy.TakeDamage(unit.damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToCombat);
    }
}