using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class CountDownTimer : MonoBehaviour
{
    float currentTime = 1;
    float startingTime = 900;
    //[SerializeField] NavMeshAgent soldier;
    [SerializeField]Text countdownText;
    [SerializeField] GameObject target;
    [SerializeField] GameObject target1;
    [SerializeField] GameObject target2;
    public NavMeshAgent[] enemy1;
    public NavMeshAgent[] enemy2;
    public NavMeshAgent[] enemy3;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        
        
       countdownText.text = currentTime.ToString();
     
            Mover();
    }

    void Mover()
    {
        if(currentTime <= 600)
        {
            
            foreach(NavMeshAgent soldier in enemy1)
            {
                soldier.SetDestination(target.transform.position);
            }
            //enemy.SetDestination(target.transform.position);
            //time2 = startingTime;
           // countdownText.text = time2.ToString();
            //time2 -= 1 * Time.deltaTime;
        }
        if(currentTime <= 300)
        {
            foreach (NavMeshAgent soldier in enemy2)
            {
                soldier.SetDestination(target1.transform.position);
            }
            //enemy.SetDestination(target.transform.position);
           // time3 = startingTime;
            //countdownText.text = time3.ToString();
            //time3 -= 1 * Time.deltaTime;
        }
        if (currentTime <= 0)
        {
            foreach (NavMeshAgent soldier in enemy3)
            {
                soldier.SetDestination(target2.transform.position);
                countdownText.text = "000";
            }
            //enemy.SetDestination(target.transform.position);
           
        }
    }
}
