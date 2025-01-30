using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;  
    // Start is called before the first frame update
    void Start()
    {
        //locking cursor to the midle of the screen and making it invisible so no obstruction in playing
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse inputs

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //Rotation around x axis (up and down)
        xRotation -= mouseY;

        //clamp the rotation
        xRotation = Mathf.Clamp(xRotation,topClamp, bottomClamp);

        //Rotation around x axis (up and down)
        yRotation += mouseX;

        //Apply rotation to the transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
