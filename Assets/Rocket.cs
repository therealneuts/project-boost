using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource rocketSound;
    bool dead = false;
    Vector3 startPosition;

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
        startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (!dead)
        {
            Rotate();
            Thrust();
        }
        else
        {
            Reset();
        }


    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //No effect
                break;

            case "Goal":
                //win level
                break;

            default:
                Death(collision);
                break;
        }   
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
            thrusterStrength = rigidBody.mass * thrusterMultiplier;


            rigidBody.AddRelativeForce(Vector3.up * thrusterStrength);

            rigidBody.freezeRotation = false;           //Resumes physics engine control of rotation
        }

        //end rocket sound on key up
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rocketSound.Stop();
        }


    }

    void Death(Collision collision)
    {
        dead = true;
        rocketSound.Stop();             //Prevents sound continuing to play when dying while holding space.

        Collider[] colliders = GetComponentsInChildren<Collider>();     //Disables collision

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void Reset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            dead = false;

            Collider[] colliders = GetComponentsInChildren<Collider>();

            foreach (Collider collider in colliders)
            {
                collider.enabled = true;
            }

            rigidBody.freezeRotation = true;
            rigidBody.velocity = Vector3.zero;
            transform.SetPositionAndRotation(startPosition, new Quaternion(0, 0, 0, 0));
            rigidBody.freezeRotation = false;
        }
    }

}
