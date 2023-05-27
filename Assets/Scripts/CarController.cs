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

    public System.Action<int, int, Quaternion> onWheelRotation;
    public System.Action<bool, bool> onBrakePressed;

    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque;
    public float maxBrake;
    public float maxSteeringAngle;
    public InputController inputController;
    public TrailRenderer[] trailRenderer; //0 - left 1 - right

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
        if(power > 0 && motorPower == 0)
        {
            SoundController.instance.PlaySound(SFXid.cochinilla);
        }
        motorPower = maxMotorTorque * (power /* * 2 - 1 */);
    }

    // 0 -> left
    // 1 -> right
    // any other value -> do nothing
    public void SetBrakingWheel(float wheel)
    {
        float velocity = rb.velocity.magnitude;
        SoundController.instance.UpdateRunVolume(velocity);

        if(wheel == 0)
        {
            if(leftBrake == 0 && velocity > 5)
            {
                SoundController.instance.PlaySound(SFXid.escarabajo);
            }
            leftBrake = maxBrake;
            rightBrake = 0;

            trailRenderer[0].emitting = true;
            trailRenderer[1].emitting = false;

            onBrakePressed?.Invoke(true, false);
        }
        else if(wheel == 1)
        {
            if(rightBrake == 0 && velocity > 5)
            {
                SoundController.instance.PlaySound(SFXid.escarabajo);
            }
            rightBrake = maxBrake;
            leftBrake = 0;

            trailRenderer[0].emitting = false;
            trailRenderer[1].emitting = true;

            onBrakePressed?.Invoke(false, true);
        }
        else
        {
            leftBrake = 0;
            rightBrake = 0;

            trailRenderer[0].emitting = false;
            trailRenderer[1].emitting = false;

            onBrakePressed?.Invoke(false, false);
        }
    }
     
    private void FixedUpdate()
    {
        for(int i = 0; i<axleInfos.Count; i++)
        {
            AxleInfo axleInfo = axleInfos[i];

            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = leftSteering;
                axleInfo.rightWheel.steerAngle = rightSteering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motorPower;
                axleInfo.rightWheel.motorTorque = motorPower;
            }
            if (axleInfo.braking)
            {
                axleInfo.leftWheel.motorTorque = -leftBrake;
                axleInfo.rightWheel.motorTorque = -rightBrake;
                if(leftBrake > 0 || rightBrake > 0)
                {
                    rb.drag = 1;
                }
                else
                {
                    rb.drag = 0.2f;
                }
            }

            ApplyLocalPositionToVisuals(i == 0, true, axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(i == 0, false, axleInfo.rightWheel);
        }
    }
     
    // finds the corresponding visual wheel
    // correctly applies the transform
    private void ApplyLocalPositionToVisuals(bool isFront, bool isLeft, WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.localRotation = Quaternion.Euler(90, collider.steerAngle, 0);
        // visualWheel.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
        
        onWheelRotation?.Invoke(isFront ? 0 : 1, isLeft ? 0 : 1, rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 5)
        {
            SoundController.instance.PlaySound(SFXid.chocazo);
        }
    }
}
