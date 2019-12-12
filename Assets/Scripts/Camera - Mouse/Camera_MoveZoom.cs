using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_MoveZoom : MonoBehaviour
{
    [Header("Camera Move XZ (WASD)")]
    [SerializeField] float xspeed = 1f;
    [SerializeField] float zspeed = 1f;
    [SerializeField] float interpolationSpeed = 10f;
    Vector3 inputVector;
    Vector3 moveSpeed = Vector3.zero;
    
    [SerializeField] Vector2 xConstrain = Vector2.zero;
    [SerializeField] Vector2 zConstrain = Vector2.zero;
    [Space]
    [Header("Camera Move Mouse")]
    [SerializeField] float moveAmount = 100f;
    [SerializeField] float edgeSize = 10f;
    [Space]
    [Header("Camera Zoom (FOV)")]
    public float zoomScrollWheelSensitivty = 5f; //sensitive zoom
    public float zoomScrollWheelSpeed = 10f; //speed zoom Scroll wheel
    float FOV; //Field of View
    public float maxFOV = 60f; //Max FOV
    public float minFOV = 40f; //Min FOV

    [SerializeField] bool MoveWithMouse = true;

    // Start is called before the first frame update
    void Start()
    {
        FOV = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveWithMouse)
        CameraMouseMove();
        CameraMove();
        CameraZoom();
    }

    void CameraMove()
    {
        inputVector = new Vector3(Input.GetAxis("Horizontal") * (xspeed * 0.5f), 0, Input.GetAxis("Vertical") * (zspeed * 0.5f));
        moveSpeed = Vector3.Lerp(moveSpeed, inputVector, Time.deltaTime * interpolationSpeed); //set movespeed

        transform.Translate(moveSpeed, Space.World);//moves in the moveSpeed vector
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xConstrain.x, xConstrain.y), transform.position.y, Mathf.Clamp(transform.position.z, zConstrain.x, zConstrain.y)); //limit movement on XZ
    }

    void CameraMouseMove()
    {
        if (Input.mousePosition.x > Screen.width - edgeSize)
        {
            moveSpeed.x += moveAmount * Time.deltaTime;
        }
        if (Input.mousePosition.x < edgeSize)
        {
            moveSpeed.x -= moveAmount * Time.deltaTime;

        }
        if (Input.mousePosition.y > Screen.height - edgeSize)
        {
            moveSpeed.z += moveAmount * Time.deltaTime;
        }
        if (Input.mousePosition.y < edgeSize)
        {
            moveSpeed.z -= moveAmount * Time.deltaTime;
        }
    }

    void CameraZoom()
    {
        FOV -= Input.GetAxis("Mouse ScrollWheel") * zoomScrollWheelSensitivty;
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, FOV, Time.deltaTime * zoomScrollWheelSpeed);

        FOV = Mathf.Clamp(FOV, minFOV, maxFOV);
    }
}
