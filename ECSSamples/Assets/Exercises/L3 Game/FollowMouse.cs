using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    Vector3 targetPosition;

    private void FixedUpdate()
    {
        Debug.Log("working");
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.localPosition = targetPosition;
    }
}
