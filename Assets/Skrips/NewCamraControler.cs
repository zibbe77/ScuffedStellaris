using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewCamraControler : MonoBehaviour
{
    private CamraControllActions cameraActions;
    private InputAction movement;
    private Transform cameraTransform;

    //Horizontal motion 
    [SerializeField] private float maxSpeed = 5f;
    private float speed;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float damping = 15f;

    //Vertical motion - Zooming
    [SerializeField] private float stepSize = 2f;
    [SerializeField] private float zoomDampening = 7.5f;
    [SerializeField] private float minHeight = 5f;
    [SerializeField] private float maxHeight = 50f;
    [SerializeField] private float zoomSpeed = 2f;

    //Rotation
    [SerializeField] private float maxRotationSpeed = 1f;
    [SerializeField] private float edgeTolerance = 0.05f;

    //value set in various functions 
    //used to update the position of the camera base object.
    private Vector3 targetPosition;
    private float zoomHeight;

    //used to track and maintain velocity w/o a rigidbody
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    [SerializeField] private bool scrollingBool = false;

    //tracks where the dragging action started
    Vector3 startDrag;

    [SerializeField] private Vector2 boundsRange = new Vector2(100, 100);

    private void Awake()
    {
        cameraActions = new CamraControllActions();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;

        targetPosition = transform.position;

    }
    private void OnEnable()
    {
        lastPosition = this.transform.position;
        movement = cameraActions.Camra.Movement;
        cameraActions.Camra.Enable();
    }
    private void OnDisable()
    {
        cameraActions.Disable();
    }

    private void Update()
    {
        GetKeyboardMovement();

        UpdateVelocity();
        UpdateBasePosition();
    }



    private void UpdateVelocity()
    {
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = this.transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputvalue = movement.ReadValue<Vector2>().x * GetCameraRight()
        + movement.ReadValue<Vector2>().y * GetCameraForward();

        inputvalue = inputvalue.normalized;

        if (inputvalue.sqrMagnitude > 0.1f)
        {
            targetPosition += inputvalue;
        }
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right;
    }
    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        return forward;
    }

    private void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        targetPosition = Vector3.zero;
    }




    //boundsRange ritrar 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 5f);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boundsRange.x * 2f, 5f, boundsRange.y * 2f));
    }

}
