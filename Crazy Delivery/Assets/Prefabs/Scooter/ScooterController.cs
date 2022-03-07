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
    [SerializeField] private float motorForce = 300f;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle = 20f;
    
    [SerializeField] private Transform centerOfMass;
    private Rigidbody _rigidbody;
    [SerializeField] private WheelCollider frontWheelCollider;
    [SerializeField] private WheelCollider backWheelCollider;

    [SerializeField] private Transform frontWheelTransform;
    [SerializeField] private Transform backWheelTransform;
    [SerializeField] private Transform BIKE;
    void Start() 
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;
    }
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
        if(verticalInput == 0)
        {
           currentBreakForce = 1 ;
        }     
        ApplyBreaking();   
    } 

    private void ApplyBreaking()
    {
        frontWheelCollider.brakeTorque = currentBreakForce;
        backWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horinzontalInput *3;
        frontWheelCollider.steerAngle = currentSteerAngle;

        if ( Vector3.Angle( Vector3.up, BIKE.up ) < 30) {
            BIKE.rotation = Quaternion.Slerp( BIKE.rotation, Quaternion.Euler( 0, BIKE.rotation.eulerAngles.y, 0 ), Time.deltaTime * 10 );
        }
        
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
            wheelTransform.rotation = rot*Quaternion.Euler(0,180,0);
        else
            wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
