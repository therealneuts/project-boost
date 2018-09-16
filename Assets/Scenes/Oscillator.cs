using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;

    [Range(0, 1)] [SerializeField] float movementFactor;
    [SerializeField] float period = 4f;

    Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update() {
        if (Mathf.Abs(period) > Mathf.Epsilon)
        {
            float cycles = Time.time / period;
            const float tau = 2 * Mathf.PI;
            float rawSineWave = Mathf.Sin(cycles * tau);



            movementFactor = (rawSineWave / 2f + 0.5f);
            Vector3 offset = movementVector * movementFactor;
            transform.position = startPos + offset;
        }
	}
}
