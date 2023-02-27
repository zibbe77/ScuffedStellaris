using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewCamraControler : MonoBehaviour
{
    // has all the variables 
    #region  variables 
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
    [SerializeField] private float maxRotationX = 45f;
    [SerializeField] private float minRotationX = 330f;

    //Edge movment
    [SerializeField] private float edgeTolerance = 0.05f;
    [SerializeField] private bool useScreenEdge = false;

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
    #endregion

    //Have all the Generally stuff and uppdates evryting
    #region Generally 
    private void Awake()
    {
        cameraActions = new CamraControllActions();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;

        targetPosition = transform.position;
    }
    private void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(this.transform);

        lastPosition = this.transform.position;
        movement = cameraActions.CamraNew.Movement;
        cameraActions.CamraNew.RotateCamera.performed += RotateCamera;
        cameraActions.CamraNew.ZoomCamera.performed += ZoomCamera;

        cameraActions.CamraNew.Enable();
    }
    private void OnDisable()
    {
        cameraActions.CamraNew.RotateCamera.performed -= RotateCamera;
        cameraActions.CamraNew.ZoomCamera.performed -= ZoomCamera;
        cameraActions.CamraNew.Disable();
    }

    private void Update()
    {
        //input
        GetKeyboardMovement();
        if (useScreenEdge) { CheckMouseAtScreenEdge(); }
        DragCamera();

        //logic
        UpdateVelocity();
        UppdateCameraPosition();
        UpdateBasePosition();
    }
    #endregion

    //controles the w,s,a,d movment in the x and z dircsions.  
    #region  Movement 

    //Gets the input from w,s,a,d and queues the movment
    private void GetKeyboardMovement()
    {
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight()
        + movement.ReadValue<Vector2>().y * GetCameraForward();

        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
        {
            targetPosition += inputValue;
        }
    }
    //Gets input from the edge of the screen and queues the movment
    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < edgeTolerance * Screen.width)
        {
            moveDirection += -GetCameraRight();
        }
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
        {
            moveDirection += GetCameraRight();
        }

        if (mousePosition.y < edgeTolerance * Screen.height)
        {
            moveDirection += -GetCameraForward();
        }
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
        {
            moveDirection += GetCameraForward();
        }

        targetPosition += moveDirection;
    }

    //lets you drag camra (has its own logic) needs the tag main camera to work
    private void DragCamera()
    {
        if (!Mouse.current.middleButton.isPressed) { return; }

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out float distance))
        {
            if (Mouse.current.middleButton.wasPressedThisFrame) { startDrag = ray.GetPoint(distance); }
            else { targetPosition += startDrag - ray.GetPoint(distance); }
        }
    }

    //Logic to get camra moving. 
    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0f;
        return right;
    }
    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        return forward;
    }
    private void UpdateVelocity()
    {
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = this.transform.position;
    }
    private void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);

            bool boundsNotHit = checkMovment();
            if (boundsNotHit)
            {
                transform.position += targetPosition * speed * Time.deltaTime;
            }
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            bool boundsNotHit = checkMovment();
            if (boundsNotHit)
            {
                transform.position += horizontalVelocity * Time.deltaTime;
            }
        }

        targetPosition = Vector3.zero;
    }

    private bool checkMovment()
    {
        bool boundsNotHit = true;
        //check x 
        if (transform.position.x + targetPosition.x * speed * Time.deltaTime > boundsRange.x)
        {
            transform.position = new Vector3(boundsRange.x, transform.position.y, transform.position.z);
            boundsNotHit = false;
        }
        else if (transform.position.x + targetPosition.x * speed * Time.deltaTime < -boundsRange.x)
        {
            transform.position = new Vector3(-boundsRange.x, transform.position.y, transform.position.z);
            boundsNotHit = false;
        }

        //check y
        if (transform.position.z + targetPosition.z * speed * Time.deltaTime > boundsRange.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, boundsRange.y);
            boundsNotHit = false;
        }
        else if (transform.position.z + targetPosition.z * speed * Time.deltaTime < -boundsRange.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -boundsRange.y);
            boundsNotHit = false;
        }

        return boundsNotHit;
    }
    #endregion 

    // rotatets the camra x and y and has an limit on x rotasion.
    #region Rotate
    private void RotateCamera(InputAction.CallbackContext inputvalue)
    {
        if (!Mouse.current.rightButton.isPressed) { return; }

        float valueX = inputvalue.ReadValue<Vector2>().x;
        float valueY = -inputvalue.ReadValue<Vector2>().y;

        Quaternion rotateAmount = Quaternion.Euler(valueY * maxRotationSpeed + transform.rotation.eulerAngles.x, valueX * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);

        // Limits the rotasion 
        if (rotateAmount.eulerAngles.x < minRotationX && rotateAmount.eulerAngles.x > minRotationX - 25)
        {
            rotateAmount.eulerAngles = new Vector3(minRotationX, rotateAmount.eulerAngles.y, rotateAmount.eulerAngles.z);
        }
        else if (rotateAmount.eulerAngles.x > maxRotationX && rotateAmount.eulerAngles.x < (maxRotationX + 25))
        {
            rotateAmount.eulerAngles = new Vector3(maxRotationX, rotateAmount.eulerAngles.y, rotateAmount.eulerAngles.z);
        }

        transform.rotation = rotateAmount;
    }
    #endregion

    //controles the zoom of the project has a min and max
    #region zoom 

    private void ZoomCamera(InputAction.CallbackContext inputvalue)
    {
        float value = -inputvalue.ReadValue<Vector2>().y / 100f;

        if (Mathf.Abs(value) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.y + value * stepSize;

            if (zoomHeight < minHeight)
            {
                zoomHeight = minHeight;
            }
            else if (zoomHeight > maxHeight)
            {
                zoomHeight = maxHeight;
            }
        }
    }

    private void UppdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);

        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;


        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        cameraTransform.LookAt(this.transform);
    }

    #endregion

    //boundsRange Draw 
    #region Devtools
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 5f);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boundsRange.x * 2f, 5f, boundsRange.y * 2f));
    }
    #endregion
}
