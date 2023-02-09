using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHud : MonoBehaviour
{
    private RectTransform transform;
    public Vector3 setTransform;

    // Start is called before the first frame update
    private void Awake()
    {
        transform = GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = setTransform;
        transform.anchoredPosition = new Vector2(setTransform.x, setTransform.y);
    }
}
