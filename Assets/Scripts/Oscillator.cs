using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon) {return;}
        float cycles = Time.time / period; //continually grows
        const float tau = Mathf.PI * 2;
        float rawSineWave = Mathf.Sin(cycles*tau); //-1 to 1

        movementFactor = (rawSineWave + 1f)/2f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition+offset;
    }
}
