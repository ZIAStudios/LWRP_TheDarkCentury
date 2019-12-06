using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InstAnim : MonoBehaviour
{
    public GameObject cursorAnim;

    
    public void Spawn(Vector3 position)
    {
        Instantiate(cursorAnim).transform.position = position;
    }

    

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);

            Vector3 ad = new Vector3(worldPoint.x, worldPoint.y, cursorAnim.transform.position.z);

            Spawn(ad);
        }




    }

}
