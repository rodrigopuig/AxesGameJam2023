using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    public CarController carController;

    public CarWheelAnimation carWheelAnimation;
    public CarBrakeAnimation carBrakeAnimation;

    private void Awake()
    {
        carController.onWheelRotation += carWheelAnimation.UpdateWheel;
        carController.onBrakePressed += carBrakeAnimation.UpdateParts;
    }
}