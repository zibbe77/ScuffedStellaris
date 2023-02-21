using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamraControler : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float SmoothingSpeed = 5f;

    [SerializeField] private Vector2 boundsRange = new Vector2(100, 100);

    private Vector3 targetPosition;
    private Vector3 input;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void HandleInput()
    {

    }





    //boundsRange ritrar 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 5f);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boundsRange.x * 2f, 5f, boundsRange.y * 2f));
    }

}
