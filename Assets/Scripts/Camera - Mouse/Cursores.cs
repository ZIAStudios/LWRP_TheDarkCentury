using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursores : MonoBehaviour
{
	Units_Selection selected;

    [SerializeField] Texture2D cursorBattle;
    [SerializeField] Texture2D cursorAtack;
    [SerializeField] Texture2D cursorDefense;
    [SerializeField] Texture2D cursorSlot;
    [SerializeField] Texture2D mainCursor;

    [SerializeField] CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] Vector2 hotSpot = Vector2.zero;

	[SerializeField] GameObject movePointParticle;
	ParticleSystem pointToMoveParticle;

	GameObject mouseOnTop;
  

    void Start()
    {
		pointToMoveParticle = movePointParticle.GetComponent<ParticleSystem>(); ;
		selected = Units_Selection.selectionInstance;

        Cursor.SetCursor(mainCursor, Vector2.zero, CursorMode.Auto);
    }
   
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 400))
		{
			mouseOnTop = hit.collider.gameObject;
		}

		if (selected.GetSelected().Count > 0 && Input.GetMouseButtonDown(1) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			pointToMoveParticle.Simulate(0.0f, true, true);
			movePointParticle.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
			pointToMoveParticle.Play();


		}
		if (mouseOnTop != null)
		{
			OnMouseExit();
			OnMouseEnter();
		}

		/*
        if (Physics.Raycast(ray2, out hit2, Mathf.Infinity))
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
                Vector3 ad = new Vector3(worldPoint.x, worldPoint.y, cursorAnim.transform.position.z);
                Spawn(ad);
            }        
        }
		*/
	}

    void OnMouseEnter(){
           
		if (mouseOnTop.tag == "SB_Slots")
		{		
		    Cursor.SetCursor(cursorSlot, Vector2.zero, CursorMode.Auto);			
		}

		if (mouseOnTop.tag == "SB_Defense")
		{
		    Cursor.SetCursor(cursorDefense, Vector2.zero, CursorMode.Auto);
		}

		if (mouseOnTop.tag == "SB_Attack")
		{
		    Cursor.SetCursor(cursorAtack, Vector2.zero, CursorMode.Auto);
		}

		if (mouseOnTop.transform.parent.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Cursor.SetCursor(cursorBattle, Vector2.zero, CursorMode.Auto);
		}


	}
        void OnMouseExit()
        {
            Cursor.SetCursor(mainCursor, Vector2.zero, CursorMode.Auto);
        }

    }




























