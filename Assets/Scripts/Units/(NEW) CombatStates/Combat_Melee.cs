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
	Transform toMovePoint;

	[HideInInspector] public bool onAttackPoint = false;

	[SerializeField] public float alertRadius = 15f;
	[SerializeField] float knockbackForce = 10f;

	Vector3 lastPos;

	public bool ignoreStates = false;
	public bool hardMoving = false;

	public enum State
	{
		normal,
		alert,
		combat,
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
		AlertState();
	}

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

		if (colliders.Length != (uint)0)
		{
			if (toMovePoint == null)
			toMovePoint = currentTarget.GetComponent<Positions>().BestPointToAttackFromTarget(transform.position);     //función para ir a por el punto del enemigo libre más cercano
			
				toMovePoint.GetComponent<TriggerAttackPoint>().movingToPoint = true;

		}
		else
		{
			toMovePoint.GetComponent<TriggerAttackPoint>().movingToPoint = false;
			currentTarget = null;
			toMovePoint = null;
		}


	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, alertRadius);
	}

}