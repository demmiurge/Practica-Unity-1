using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayerControllerV1 : MonoBehaviour
{
    float m_Yaw;
    float m_Pitch;

    [Space(5)]
    [Header("Rotation speed")]
    [Space(10)]
    [Tooltip("Horizontal rotation speed")]
    public float m_YawRotationSpeed;
    [Tooltip("Vertical rotation speed")]
    public float m_PitchRotationSpeed;

    [Space(5)]
    [Header("Camera vertical rotation limitation")]
    [Space(10)]
    [Tooltip("Maximum lift height stop")]
    public float m_MaxPitch;
    [Tooltip("Minimum lift height stop")]
    public float m_MinPitch;

    [Space(5)]
    [Header("Camera vertical rotation limitation")]
    [Space(10)]
    [Tooltip("The GameObject that performs the vertical flip")]
    public Transform m_PitchController;
    [Tooltip("Invert horizontal rotation controls")]
    public bool m_UseYawInverted;
    [Tooltip("Invert Vertical rotation controls")]
    public bool m_UsePitchInverted = true;

    [Space(5)]
    [Header("Character controller and character speeds")]
    [Space(10)]
    [Tooltip("Character standard speed")]
    public float m_Speed;
    [Tooltip("Standard speed multiplier, for when you sprint")]
    public float m_FastSpeedMultiplier = 1.5f;
    [Tooltip("Character jump speed")]
    public float m_JumpSpeed = 10.0f;

    [Space(5)]
    [Header("Character controls")]
    [Space(10)]
    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_RunKeyCode = KeyCode.LeftShift;

    float m_VerticalSpeed = 0.0f; //
    bool m_OnGround = true; // No matter this state

    [Space(5)]
    [Header("Cameras and FOV settings")]
    [Space(10)]
    public Camera m_GeneralCamera;
    public Camera m_WeaponCamera;
    public float m_NormalMovementFOV;
    public float m_RunMovementFOV;
    public float m_TransitionStepperFOV = 0.1f;
    public float m_SumRateRunningFOV = 10.0f;

    [Space(5)]
    [Header("Character jump settings and tolerances")]
    [Space(10)]
    public float m_MinTimeInTheAir = 0.5f;
    float m_ElapsedTimeInTheAir;
    public float m_MaxHeightSpeedTolerance = 0.005f;
    public float m_MinHeightSpeedTolerance = -0.005f;
    bool m_CanJump;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
