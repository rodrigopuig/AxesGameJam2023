using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
     
public class CarController : MonoBehaviour {
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public InputController inputController;

    private float leftSteering;
    private float rightSteering;
    private float motorPower;

    private void Start()
    {
        inputController.SetListener(CustomAction.ActionID.TurnWheel_Left, SetLeftWheelRotation);
        inputController.SetListener(CustomAction.ActionID.TurnWheel_Right, SetRightWheelRotation);
        inputController.SetListener(CustomAction.ActionID.Accelerate, SetMotorPower);
    }

    // Expects values from 0-1
    public void SetLeftWheelRotation(float rotationAngle)
    {
        leftSteering = maxSteeringAngle * (rotationAngle * 2 - 1);
    }

    public void SetRightWheelRotation(float rotationAngle)
    {
        rightSteering = maxSteeringAngle * (rotationAngle * 2 - 1);
    }

    public void SetMotorPower(float power)
    {
        motorPower = maxMotorTorque * (power /* * 2 - 1 */);
    }
     
    private void FixedUpdate()
    {
        // float motor = maxMotorTorque * Input.GetAxis("Vertical");
        // float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
     
        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = leftSteering;
                axleInfo.rightWheel.steerAngle = rightSteering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motorPower;
                axleInfo.rightWheel.motorTorque = motorPower;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
     
    // finds the corresponding visual wheel
    // correctly applies the transform
    private void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
        
    }
}
