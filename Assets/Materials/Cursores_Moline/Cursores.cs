using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursores : MonoBehaviour
{
    /*
    public void Spawn(Vector3 position)
    {
        Instantiate(cursorAnim).transform.position = position;
    }
    */
    //Custom Cursor Setup

    // [SerializeField]
    public Texture2D cursorBattle;
    public Texture2D cursorAtack;
    public Texture2D cursorDefense;
    public Texture2D cursorSlot;
    public Texture2D mainCursor;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    // public GameObject cursorAnim;
    bool collider;

  

    void Start()
    {
        Cursor.SetCursor(mainCursor, Vector2.zero, CursorMode.Auto);
    }

   

   
    void Update()
    {
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit2;
        // OnMouseEnter();

        /*
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit2;

        if (Physics.Raycast(ray2, out hit2, Mathf.Infinity))
        {
            print(hit2.collider.gameObject.tag);

            

            /*
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);

                Vector3 ad = new Vector3(worldPoint.x, worldPoint.y, cursorAnim.transform.position.z);

                Spawn(ad);
            }
          */



        // }

    }
    void OnMouseEnter(){
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit2;
        if (Physics.Raycast(ray2, out hit2, Mathf.Infinity))
        {
            
                if (hit2.collider.gameObject.tag == "SB_Slots")
                {
                
                    Cursor.SetCursor(cursorSlot, Vector2.zero, CursorMode.Auto);
                
                
                }
                else Cursor.SetCursor(mainCursor, Vector2.zero, CursorMode.Auto);
                if (hit2.collider.gameObject.tag == "SB_Defense")
                {
                    Cursor.SetCursor(cursorDefense, Vector2.zero, CursorMode.Auto);
                }
                else Cursor.SetCursor(mainCursor, Vector2.zero, CursorMode.Auto);
                if (hit2.collider.gameObject.tag == "SB_Attack")
                {
                    Cursor.SetCursor(cursorAtack, Vector2.zero, CursorMode.Auto);
                }
                else Cursor.SetCursor(mainCursor, Vector2.zero, CursorMode.Auto);

        }
           
       
    }
        void OnMouseExit()
        {


            Cursor.SetCursor(mainCursor, Vector2.zero, CursorMode.Auto);

        }

    }




























