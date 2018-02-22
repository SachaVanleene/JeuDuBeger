using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAimCamera : MonoBehaviour
{
    public GameObject target;
    public GameObject head;
    public GameObject crosshair;
    public float rotateSpeed = 5;
    Vector3 offset;
    Vector3 offset2;

    void Start()
    {
        offset = target.transform.position - transform.position;
        offset2 = target.transform.position - crosshair.transform.position;
    }

    void LateUpdate()
    {
          float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;

          target.transform.Rotate(0, horizontal, 0);
          float desiredAngle = target.transform.eulerAngles.y;
          float desiredVerticalAngle = target.transform.eulerAngles.x;
          Quaternion rotation = Quaternion.Euler(desiredVerticalAngle, desiredAngle, 0);
          transform.position = target.transform.position - (rotation * offset);
          crosshair.transform.position = target.transform.position - (rotation * offset2);
          transform.LookAt(crosshair.transform);
    }
}
