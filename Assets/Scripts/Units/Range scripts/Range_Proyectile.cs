using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range_Proyectile : MonoBehaviour
{
    public LayerMask layerToHit;

    [HideInInspector] public int proyectileDamage;

    float cooldown = 5f;
    float timer = 0;

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                gameObject.SetActive(false);

                timer = cooldown;

            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.layer == layerToHit)
        {
            //Debug.Log("Hit the layer " + layerToHit);

            col.gameObject.GetComponent<Unit_Base>().TakeDamage(proyectileDamage);
            Debug.Log("Done " + proyectileDamage + " damage to " + col.gameObject.name);

			gameObject.SetActive(false);

		}

	}
        
}
