using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Rotate the camera around the cube using the middle mouse button, Q and E
    //Move the camera around the scene using WASD
    [SerializeField] Transform centerCube;
    [SerializeField] Camera mainCamera;
    [SerializeField] float roatationSensitivity, moveSensitivity, zoomSensitivity;
    Rigidbody rb;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        centerCube.position = new Vector3(24 / 2, 0, 24 / 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
            centerCube.Rotate(-Vector3.up, roatationSensitivity * Time.fixedDeltaTime);
        if (Input.GetKey(KeyCode.E))
            centerCube.Rotate(Vector3.up, roatationSensitivity * Time.fixedDeltaTime);

        float z = Input.GetAxisRaw("Vertical");
        float x = Input.GetAxisRaw("Horizontal");

        Vector3 direction = (x * transform.right + z * transform.forward).normalized;
        rb.velocity = Vector3.zero;
        rb.MovePosition(rb.position + direction * moveSensitivity * Time.fixedDeltaTime);


        //Camera zoom and zoom limitations
        float y = Input.GetAxis("Mouse ScrollWheel");
        Vector3 cameraZoom = new Vector3(0, y, -y);

        if (mainCamera.transform.localPosition.y < 4.42 && mainCamera.transform.localPosition.z > -3.74)
        {
            mainCamera.transform.localPosition = new Vector3(0, 4.45f, -3.75f);
        }
        if(mainCamera.transform.localPosition.y > 30 && mainCamera.transform.localPosition.z < -30)
        {
            mainCamera.transform.localPosition = new Vector3(0, 30f, -30f);

        }

        mainCamera.transform.localPosition -= cameraZoom * Time.fixedDeltaTime * zoomSensitivity;

    }
}
