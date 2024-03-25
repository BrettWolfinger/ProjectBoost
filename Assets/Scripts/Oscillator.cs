using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to attach to obstacles in the game world that will make
//them move back and forth 
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;
    Vector3 startingPosition;
    float movementFactor;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    //Smoothly oscillate between the 
    void Update()
    {
        //Do nothing if period is ~0
        if(period <= Mathf.Epsilon) {return;}
        float cycles = Time.time / period; //continually grows
        const float tau = Mathf.PI * 2;
        float rawSineWave = Mathf.Sin(cycles*tau); //-1 to 1

        //Scales sine value to be 0 to 2 and divides by 2 so you get 
        //a fraction between 0 and 1 representing how much of the movementVector
        //should be used
        movementFactor = (rawSineWave + 1f)/2f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition+offset;
    }
}
