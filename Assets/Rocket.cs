using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource rocketSound;
    float fullRotationTime = 2f;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    private void ProcessInput()
    {

        //Defines thrust behavior
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rocketSound.Play();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            rocketSound.Stop();
        }

        //Defines roation behavior
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            float rotationAngle = (360f / fullRotationTime) * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, rotationAngle));
        }
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            float rotationAngle = (-360f / fullRotationTime) * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, rotationAngle));
        }
        //-------------------------------
    }
}
