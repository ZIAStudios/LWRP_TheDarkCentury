using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] float flagRadius = 5f;
    public Zone.STATE flagState;
    [Space]
    public LayerMask aiLayer;
    public LayerMask playerLayer;

    [SerializeField] int aiOnTheFlag = -1;
    [SerializeField] int playerOnTheFlag = -1;

    Collider[] aiFlagCollider;
    Collider[] playerFlagCollider;

	GameObject playerFlag;
	GameObject enemyFlag;
    GameObject neutralFlag;


    private void Start()
	{
		playerFlag = gameObject.transform.GetChild(0).gameObject;
		enemyFlag = gameObject.transform.GetChild(1).gameObject;
        neutralFlag = gameObject.transform.GetChild(2).gameObject;

        neutralFlag.SetActive(true);
        playerFlag.SetActive(false);
		enemyFlag.SetActive(false);
	}

	void Update()
    {
        FlagCapture();

		if (flagState == Zone.STATE.Player)
		{
			playerFlag.SetActive(true);
			enemyFlag.SetActive(false);
            neutralFlag.SetActive(false);
		}

		if (flagState == Zone.STATE.AI)
		{
			playerFlag.SetActive(false);
			enemyFlag.SetActive(true);
            neutralFlag.SetActive(false);

        }

    }

    void FlagCapture()
    {
        aiFlagCollider = Physics.OverlapSphere (transform.position, flagRadius, aiLayer);
        playerFlagCollider = Physics.OverlapSphere(transform.position, flagRadius, playerLayer);

        for (int i = 0; i < aiFlagCollider.Length; i++)
        {
            if (aiFlagCollider[i].tag == "E_Aldeano")
            {
                aiOnTheFlag = i;
            }
            
        }
        for (int i = 0; i < playerFlagCollider.Length; i++)
        {
            if (playerFlagCollider[i].tag ==  "P_Aldeano") //---------------------------------CAMBIAR / INCLUIR TAG VILLAGER ENEMIGO
            {
                playerOnTheFlag = i;
            }

        }

        if (aiOnTheFlag > -1 && playerOnTheFlag == -1)
        {
            flagState = Zone.STATE.AI;
        }        
        else if (playerOnTheFlag > -1 && aiOnTheFlag == -1)
        {
            flagState = Zone.STATE.Player;

        }
        else
        {
            aiOnTheFlag = -1;
            playerOnTheFlag = -1;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, flagRadius);
    }

}
