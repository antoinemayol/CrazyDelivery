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
        backWheelCollider.motorTorque = verticalInput * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();   
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
        
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontWheelCollider, frontWheelTransform,false);
        UpdateSingleWheel(backWheelCollider, backWheelTransform,true);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform,bool backWheel)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        if(!backWheel)
            wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
