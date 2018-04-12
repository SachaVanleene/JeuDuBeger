using UnityEngine;
using System.Collections;

public class RotateBehaviour : MonoBehaviour {

    public Vector3 RotationAmount;

	void Update () 
    {
        transform.Rotate(RotationAmount * Time.deltaTime);
	}
}
