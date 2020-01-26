using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class CountDownTimer : MonoBehaviour
{
    float currentTime = 1;
    float startingTime = 10;
    //[SerializeField] NavMeshAgent soldier;
    [SerializeField]Text countdownText;
    [SerializeField] GameObject target;
    [SerializeField] GameObject target1;
    [SerializeField] GameObject target2;
    public NavMeshAgent[] enemy1;
    public NavMeshAgent[] enemy2;
    public NavMeshAgent[] enemy3;
    public GameObject Enemigos1;
    public GameObject Enemigos2;
    public GameObject Enemigos3;
    public GameObject en1;
    public GameObject en2;
    public GameObject en3;
    public GameObject en4;
    public GameObject en5;
    public GameObject en6;
    public GameObject en7;
    public GameObject en8;
    public GameObject en9;
    public GameObject en10;
    public GameObject en11;
    public GameObject en12;

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
        if(!en1.activeInHierarchy  && !en2.activeInHierarchy && !en3.activeInHierarchy && !en4.activeInHierarchy && !en5.activeInHierarchy && !en6.activeInHierarchy && !en7.activeInHierarchy && !en8.activeInHierarchy && !en9.activeInHierarchy && !en10.activeInHierarchy && !en11.activeInHierarchy && !en12.activeInHierarchy)
        {
            print("quiero pasar");
            SceneManager.LoadScene("Mapa_3");
        }
    }

    void Mover()
    {
        if(currentTime <= 5)
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
        if(currentTime <= 3)
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
        if (currentTime <= 1)
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
