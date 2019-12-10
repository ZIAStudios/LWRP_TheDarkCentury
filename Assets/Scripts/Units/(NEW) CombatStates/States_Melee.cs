using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class States_Melee : MonoBehaviour
{
    NavMeshAgent agent;
	Animator anim;
    Unit_Base unit;

	[SerializeField] LayerMask maskToChase;

	public GameObject bestTarget;
    Transform toMovePoint;

    [HideInInspector] public bool onAttackPoint = false;

	[SerializeField] float alertRadius = 5f;
	[SerializeField] float knockbackForce = 10f;

	Vector3 lastPos;

	public bool ignoreStates = false;
	bool hardMoving = false;

	public enum State
    {
        normal,
        alert,
        combat,
    }
    public State state;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
        unit = GetComponent<Unit_Base>();
    }



	void Update()
    {
		if (ignoreStates && !agent.hasPath)
		{
			ignoreStates = false;
		}

		if (bestTarget == null && state == State.alert && toMovePoint!= null)
		{
			MoveTo(toMovePoint.position);
			hardMoving = true;
		}
		else
		{
			hardMoving = false;
		}

		Animations();

		if (!ignoreStates)
		{
			CombatState();
			AlertState();
		}

        FaceTarget();
        MoveStates();

    }

    #region MoveStates() - 
    //Diferentes estados de movimiento de ñl  tropa dependiendo de su estado
    private void MoveStates()
    {
        switch (state)
        {
            case State.normal:
                unit.MoveToPoint();
                break;

            case State.alert:
                MoveTo(toMovePoint.position);
                break;

            case State.combat:
                MoveTo(transform.position);
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
        if (bestTarget != null)
        {
            Vector3 direction = (bestTarget.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
    #endregion

    #region AlertState() - 

    public void SetEnemy(GameObject target)
    {
            state = State.alert;
            toMovePoint = target.GetComponent<Positions>().BestPointToAttackFromTarget(transform.position);     //función para ir a por el punto del enemigo libre más cercano

    }

    void AlertState()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, alertRadius, maskToChase); //Array de colldiers para detectar enemigos
        int lowestValue = 20;                   //Valor que nunca se puede superar (el maximo valos es el máximo de tropas atacando)

		//if (bestTarget == null /*&& Vector3.Dis*/) //PONER QUE SE MUEVA, SI PASA DE ESA DISTANCIA NO HACE CASO AL RESTO
		if (!hardMoving)
		{
			foreach (Collider item in colliders)    //Determina cual es el target con menos unidades atacandole
			{
				int value = item.gameObject.GetComponent<Positions>().positionsInUse;
				if (value < lowestValue)
				{
					lowestValue = value;
					bestTarget = item.gameObject;
				}
			}
		}

        if (colliders.Length != (uint)0)
        {
			if (state != State.combat)
            SetEnemy(bestTarget);
        }
        else
        {
            if (state != State.normal)
            {
				if (bestTarget.layer != 0)	//CUANDO HARDCODEA ATAQUE COGE DE TARGET AL SELECTOR DEL TARGET, QUE TIENE LAYER 0 (Default) ?!?!?!CUIDAO?!?!!?
                MoveTo(transform.position);     //que cuando pase de estado de alerta a normal, se quede quieto, no busque la última posición
            }
            bestTarget = null;
            toMovePoint = null;
            state = State.normal;
        }
    }
    #endregion

    #region CombatState() - 

    public void CombatState()
    {
        if (bestTarget != null)
        {
			if (toMovePoint != null)
			{
				float distance = Vector3.SqrMagnitude(toMovePoint.position - transform.position);
				if (distance <= gameObject.GetComponent<CapsuleCollider>().radius)
				{
					state = State.combat;
				}
	
				if (state == State.combat)
				{
					if (distance > 0.2f + gameObject.GetComponent<CapsuleCollider>().radius)
					{
						state = State.alert;
					}
				}
			}
        }
    }
    #endregion


	//Función para activar en la animación para hacer daño, ya que TakeDamage es de uno mismo y no se puede hacer por animación
	void HitEnemy()
	{		
		var enemy = bestTarget.GetComponent<Unit_Base>();

        enemy.anim.SetBool("TakeDamage", true);
		enemy.TakeDamage(unit.damage);

		float hitSound = Random.Range(0, 4);
		if (hitSound <= 2)
		{
			//FindObjectOfType<AudioManager>().Play("Combate");
		}
		else
		{
			//FindObjectOfType<AudioManager>().Play("Combate1");
		}

		bestTarget.GetComponent<Rigidbody>().AddExplosionForce(knockbackForce * 10, gameObject.transform.position, alertRadius, 3f, ForceMode.Impulse);
	
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
		if (state == State.alert)
		{
			anim.SetBool("Attacking", false);

		}
		if (state == State.combat)
		{
			anim.SetBool("Attacking", true);
			anim.SetBool("Move", false);

		}
		if (state == State.normal)
		{
			anim.SetBool("Attacking", false);
		}
	}

	private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }

}
