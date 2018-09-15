using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource rocketSound;

    //Defines the time it should take to make a full 360 degree rotation.
    [SerializeField] float fullRotationTime = 2f;
    [SerializeField] float thrusterMultiplier = 100f;
    float thrusterStrength;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();

        rocketSound.Stop();         //Fixes annoying crackling bug

        thrusterStrength = rigidBody.mass*thrusterMultiplier;
	}
	
	// Update is called once per frame
	void Update () {
        thrusterStrength = rigidBody.mass * thrusterMultiplier;
        Rotate();
        Thrust();
    }

    private void Rotate()
    {
        //Defines rotation behavior
        float rotationPerFrame = 360f / fullRotationTime * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            //Uses deltaTime to calculate magnitude of rotation
            transform.Rotate(new Vector3(0, 0, rotationPerFrame));
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, 0, -rotationPerFrame));
        }


    }

    private void Thrust()
    {
        //Defines thrust behavior

        //Start rocket sound on key down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rocketSound.Play();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.freezeRotation = true;            //Takes manual control of rotation

            rigidBody.AddRelativeForce(Vector3.up * thrusterStrength);

            rigidBody.freezeRotation = false;           //Resumes physics engine control of rotation
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (rigidBody.velocity.y > 0)
            {
                rigidBody.AddRelativeForce(Vector3.up * (-rigidBody.velocity.y * thrusterStrength)/20f);
            }
        }

        //end rocket sound on key up
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rocketSound.Stop();
        }


    }
}
