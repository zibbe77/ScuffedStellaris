using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCustom : MonoBehaviour
{
    public Transform cameraTransform;

    public float fastSpeed;
    public float normalSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 NewZoom;

    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        NewZoom = cameraTransform.localPosition;
    }

    void Update()
    {
        HandleMovementInput();
    }

    public void HandleMovementInput()
    {
        //snabbare eller långasmare att man rör sig 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        //hanterar rörelsen fram, bakåt, och sidorna 
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }


        //Hanterar rotation 
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        //zoom 
        if (Input.GetKey(KeyCode.R))
        {
            NewZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            NewZoom -= zoomAmount;
        }

        //seter inputesen 
        transform.position = Vector3.Lerp(transform.position, newPosition, movementTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, NewZoom, Time.deltaTime * movementTime);
    }
}

