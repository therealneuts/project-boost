using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    //Fields
    Rigidbody rigidBody;
    AudioSource audioSource;
    Vector3 startPosition;
    float thrusterStrength;


    //States control game flow
    enum States { Alive, Dying, Transcending};
    States state;

    //Defines the time it should take to make a full 360 degree rotation.
    [SerializeField] float fullRotationTime = 2f;
    [SerializeField] float thrusterMultiplier = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip WinChime;


	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        audioSource.Stop();         //Fixes annoying crackling bug

        thrusterStrength = rigidBody.mass*thrusterMultiplier;
        startPosition = transform.position;
        state = States.Alive;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (state == States.Alive)
        {
            HandleRotation();
            HandleThrust();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != States.Alive) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //No effect
                break;

            case "Goal":
                Win();
                Invoke("LoadNextScene", 2f);
                break;

            default:
                Die(collision);
                Invoke("Reset", 2f);
                break;
        }   
    }

    private void Win()
    {
        audioSource.PlayOneShot(WinChime);
        state = States.Transcending;
    }

    private void HandleRotation()
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

    private void HandleThrust()
    {
        //Defines thrust behavior

        //Start rocket sound on key down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(mainEngine);
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
            audioSource.Stop();
        }


    }

    void Die(Collision collision)
    {
        state = States.Dying;
        audioSource.Stop();             //Prevents sound continuing to play when dying while holding space.
        audioSource.PlayOneShot(explosion);

        Collider[] colliders = GetComponentsInChildren<Collider>();     //Disables collision

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void Reset()
    {
        state = States.Alive;

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

    void LoadNextScene()
    {
        SceneManager.LoadScene(1);      //todo allow for more levels
    }
}
