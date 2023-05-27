using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    public CarController carController;

    public CarWheelAnimation carWheelAnimation;
    public CarBrakeAnimation carBrakeAnimation;
    public CarButterflyAnimation carButterflyAnimation;

    private void Start()
    {
        carController.onWheelRotation += carWheelAnimation.UpdateWheel;
        carController.onBrakePressed += carBrakeAnimation.UpdateParts;
        carController.inputController.SetListener_onAddCombo(CustomAction.ActionID.Accelerate, carButterflyAnimation.Wing);
        carController.inputController.SetListener_onCompleteCombo(CustomAction.ActionID.Accelerate, carButterflyAnimation.CompleteCombo);
    }
}