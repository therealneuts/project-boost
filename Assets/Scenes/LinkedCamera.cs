using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedCamera : MonoBehaviour {

    public Transform playerTransform;
    public Transform cameraTransform;

    Vector3 offset;



	// Use this for initialization
	void Start () {
        playerTransform = transform.Find("Rocket Ship");
        cameraTransform = transform.Find("Main Camera");
        offset = cameraTransform.position - playerTransform.position;
	}
	
	// Update is called once per frame
	void Update () {
 
	}

    private void LateUpdate()
    {
        cameraTransform.position = playerTransform.position + offset;
    }
}
