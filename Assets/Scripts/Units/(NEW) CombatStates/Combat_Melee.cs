using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Combat_Melee : MonoBehaviour
{
	#region Variables

	NavMeshAgent agent;
	Animator anim;
	Unit_Base unit;

	[SerializeField] public LayerMask maskToChase;

	public GameObject currentTarget;
	public Transform toMovePoint;

	[HideInInspector] public bool onAttackPoint = false;

	[SerializeField] public float alertRadius = 15f;
	[SerializeField] float knockbackForce = 10f;

	Vector3 lastPos;

	public bool ignoreStates = false;
	public bool clickOnEnemy = false;

	public enum State
	{
		normal,
		alert,
		combat,
        forceMove,
        onEnemy
	}
	public State state;

	#endregion

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		unit = GetComponent<Unit_Base>();
	}

	void Start()
	{

	}


	void Update()
	{   
        //Cuando se haga click en una tropa enemiga y esta muera, que vuelva a estado normal
        if (clickOnEnemy && !toMovePoint.gameObject.activeInHierarchy)
        {
            toMovePoint = null;
            clickOnEnemy = false;
        }

		AlertState();
        MoveStates();

	}


    #region MoveStates()

    private void MoveStates()
    {
        switch (state)
        {
            case State.alert:
                MoveTo(toMovePoint.position);
                break;

            case State.combat:
                MoveTo(transform.position);
                break;

            case State.forceMove:

                break;

            case State.onEnemy:

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

    void FaceTarget() //Face the target
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
    #endregion

    #region AlertState() - State where detects the enemy
    void AlertState()
	{
		CheckEnemyAtPosition(transform.position);
	}

	public void CheckEnemyAtPosition(Vector3 positiontocheck)
	{
		Collider[] colliders = Physics.OverlapSphere(positiontocheck, alertRadius, maskToChase); //Array de colldiers para detectar enemigos
		int lowestValue = 20;                   //Valor que nunca se puede superar (el maximo valos es el máximo de tropas atacando)

		foreach (Collider item in colliders)    //Determina cual es el target con menos unidades atacandole
		{
			int value = item.gameObject.GetComponent<Positions>().positionsInUse;
			if (value < lowestValue)
			{
				lowestValue = value;
				currentTarget = item.gameObject;
			}
		}

        if (!clickOnEnemy)
        {
            if (colliders.Length != (uint)0)
            {
                if (toMovePoint == null)
                {
                    //función para ir a por el punto del enemigo libre más cercano
                    toMovePoint = currentTarget.GetComponent<Positions>().BestPointToAttackFromTarget(transform.position);

                    toMovePoint.GetComponent<TriggerAttackPoint>().movingToPoint = true;

                    state = State.alert;
                }

            }
            else
            {
                toMovePoint.GetComponent<TriggerAttackPoint>().movingToPoint = false;
                currentTarget = null;
                toMovePoint = null;
                //Cambio de State a no ser que sea forceState (fuerza  amoverse da igual el estado)
                if (state != State.forceMove)
                state = State.normal;
            }
        }

	}

    //Llama desde UNIT_BASE y setea al clickar sobre el enemigo
    public void SetEnemy(GameObject target)
    {
        toMovePoint = target.GetComponent<Positions>().BestPointToAttackFromTarget(transform.position);
        if (!toMovePoint.GetComponent<TriggerAttackPoint>().movingToPoint)
            print("false");
        toMovePoint.GetComponent<TriggerAttackPoint>().movingToPoint = true;

    }
    #endregion


    private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, alertRadius);
	}

}