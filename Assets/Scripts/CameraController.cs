using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform car;

    void Start()
    {
        car = FindObjectOfType<CarController>().transform;
    }

    void Update()
    {
        transform.position = car.position;
    }
}
