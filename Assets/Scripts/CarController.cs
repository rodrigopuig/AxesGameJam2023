using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    public bool braking;
}
     
public class CarController : MonoBehaviour {
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque;
    public float maxBrake;
    public float maxSteeringAngle;
    public InputController inputController;

    [HideInInspector]public Rigidbody rb;

    private float leftSteering;
    private float rightSteering;
    private float motorPower;
    private float leftBrake;
    private float rightBrake;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        inputController.SetListener(CustomAction.ActionID.TurnWheel_Left, SetLeftWheelRotation);
        inputController.SetListener(CustomAction.ActionID.TurnWheel_Right, SetRightWheelRotation);
        inputController.SetListener(CustomAction.ActionID.Accelerate, SetMotorPower);
        inputController.SetListener(CustomAction.ActionID.Brake, SetBrakingWheel);
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

    // 0 -> left
    // 1 -> right
    // any other value -> do nothing
    public void SetBrakingWheel(float wheel)
    {
        if(wheel == 0)
        {
            leftBrake = maxBrake;
            rightBrake = 0;
        }
        else if(wheel == 1)
        {
            rightBrake = maxBrake;
            leftBrake = 0;
        }
        else
        {
            leftBrake = 0;
            rightBrake = 0;
        }
    }
     
    private void FixedUpdate()
    {
        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = leftSteering;
                axleInfo.rightWheel.steerAngle = rightSteering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motorPower;
                axleInfo.rightWheel.motorTorque = motorPower;
            }
            if(axleInfo.braking)
            {
                axleInfo.leftWheel.motorTorque = -leftBrake;
                axleInfo.rightWheel.motorTorque = -rightBrake;
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
