using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScooterController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horinzontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontWheelCollider;
    [SerializeField] private WheelCollider backWheelCollider;

    [SerializeField] private Transform frontWheelTransform;
    [SerializeField] private Transform backWheelTransform;
    [SerializeField] private Transform steerTransform;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horinzontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontWheelCollider.motorTorque = verticalInput * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        if (isBreaking)
        {
            ApplyBreaking();
        }
    }

    private void ApplyBreaking()
    {
        frontWheelCollider.brakeTorque = currentBreakForce;
        backWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horinzontalInput;
        frontWheelCollider.steerAngle = currentSteerAngle;
    //    steerTransform.rotation = Quaternion.Euler(250,90,currentSteerAngle);
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontWheelCollider, frontWheelTransform);
        UpdateSingleWheel(backWheelCollider, backWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
