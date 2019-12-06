using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : MonoBehaviour
{
    [SerializeField] float distance = 2f; //Distancia de puntos al jugador

    [HideInInspector] public Transform[] positionsToAttack = new Transform[6];  //array con las posiciones que se van a usar

    public bool[] emptyPositions = new bool[6];                                 //array que va variando constantemente

    [HideInInspector] public int pos0, pos1, pos2, pos3, pos4, pos5;
    public int positionsInUse = 0;

    private void Awake()
    {
        #region Point to add to the list (take child from gameobject)
        positionsToAttack[0] = gameObject.transform.GetChild(2).GetChild(0);
        positionsToAttack[1] = gameObject.transform.GetChild(2).GetChild(1);
        positionsToAttack[2] = gameObject.transform.GetChild(2).GetChild(2);
        positionsToAttack[3] = gameObject.transform.GetChild(2).GetChild(3);
        positionsToAttack[4] = gameObject.transform.GetChild(2).GetChild(4);
        positionsToAttack[5] = gameObject.transform.GetChild(2).GetChild(5);
        #endregion
    }


    void Update()
    {
        AttackPointMovable();
       
            int add0 = (emptyPositions[0] == false) ? 1 : 0;
            int add1 = (emptyPositions[1] == false) ? 1 : 0;
            int add2 = (emptyPositions[2] == false) ? 1 : 0;
            int add3 = (emptyPositions[3] == false) ? 1 : 0;
            int add4 = (emptyPositions[4] == false) ? 1 : 0;
            int add5 = (emptyPositions[5] == false) ? 1 : 0;

            positionsInUse = add0 + add1 + add2 + add3 + add4 + add5;

    }

    #region BestPointToAttackFromTarget(Vector 3...) - It's called on the enemy, picks the closest point from him

    public Transform BestPointToAttackFromTarget(Vector3 originalPosition)
    {
        Transform bestPoint = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < positionsToAttack.Length; i++)
        {
            if (positionsToAttack[i] && emptyPositions[i])
            {
                float distance = Vector3.SqrMagnitude(positionsToAttack[i].position - originalPosition);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestPoint = positionsToAttack[i];
                }

            }

        }
        return bestPoint;

    }
    #endregion


    #region AttackPointMovable() - Attack points are movable (Edit on UNITY EDITOR)
    //Permite variar la distancia que hay de cada punto con respecto al personaje, al mismo tiempo
    void AttackPointMovable()
    {
        for (int i = 0; i < positionsToAttack.Length; i++)
        {
            var heading = positionsToAttack[i].position - transform.position;
            var direction0 = heading / heading.magnitude;
            positionsToAttack[i].position = transform.position + direction0 * distance;
        }

    }
    #endregion

}
