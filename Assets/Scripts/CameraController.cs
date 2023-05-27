using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lookAhead;
    public float smooth;
    private CarController car;

    void Start()
    {
        car = FindObjectOfType<CarController>();
    }

    void Update()
    {
        if(car.rb != null)
        {
            transform.position = Vector3.Lerp(transform.position, car.transform.position + car.rb.velocity * lookAhead, smooth);
        }
    }
}
