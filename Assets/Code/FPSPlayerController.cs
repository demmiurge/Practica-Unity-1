using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayerController : MonoBehaviour
{
    float m_Yaw;
    float m_Pitch;
    public float m_YawRotationSpeed;
    public float m_PitchRotationSpeed;

    public float m_MinPitch;
    public float m_MaxPitch;

    public Transform m_PitchController;
    public bool m_UseYawInverted;
    public bool m_UsePitchInverted;

    public CharacterController m_CharacterController;
    public float m_Speed;
    public float m_FastSpeedMultiplier = 1.5f;
    public KeyCode m_LeftKeyCode;
    public KeyCode m_RightKeyCode;
    public KeyCode m_UpKeyCode;
    public KeyCode m_DownKeyCode;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_RunKeyCode = KeyCode.LeftShift;

    float m_VerticalSpeed = 0.0f;
    bool m_OnGround = true; // No matter this state

    public Camera m_Camera;
    public float m_NormalMovementFOV;
    public float m_RunMovementFOV;
    public float m_TransitionStepperFOV = 0.1f;
    public float m_SumRateRunningFOV = 10.0f;

    public float m_JumpSpeed = 10.0f;

    // Timer
    public float m_TimerBetweenHops = 1.0f;
    float m_TimerHopsRemaining;

    // float m_LimitTimeToAir = 0.2f; // Hay que hacer un timer

    // Start is called before the first frame update
    void Start()
    {
        m_Yaw = transform.rotation.y;
        m_Pitch = m_PitchController.localRotation.x;

        SetFOVIfParametersAreEmpty();
    }

    private void SetFOVIfParametersAreEmpty()
    {
        if (m_Camera && m_NormalMovementFOV == 0 || m_RunMovementFOV == 0)
        {
            m_NormalMovementFOV = m_NormalMovementFOV == 0 ? m_Camera.fieldOfView : m_NormalMovementFOV;
            m_RunMovementFOV = m_NormalMovementFOV + m_SumRateRunningFOV;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 l_RightDirection = transform.right;
        Vector3 l_ForwardDirection = transform.forward;
        Vector3 l_Direction = Vector3.zero;

        float l_Speed = m_Speed;

        float l_MouseX = Input.GetAxis("Mouse X"); // It is the old Unity input
        float l_MouseY = Input.GetAxis("Mouse Y");

        // Movement
        if (Input.GetKey(m_UpKeyCode)) l_Direction = l_ForwardDirection;
        if (Input.GetKey(m_DownKeyCode)) l_Direction = -l_ForwardDirection;
        if (Input.GetKey(m_RightKeyCode)) l_Direction += l_RightDirection;
        if (Input.GetKey(m_LeftKeyCode)) l_Direction -= l_RightDirection;

        // Jump
        if (Input.GetKeyDown(m_JumpKeyCode) && m_OnGround)
        {
            m_VerticalSpeed = m_JumpSpeed;
            m_TimerHopsRemaining = m_TimerBetweenHops;
        }

        // FOB control
        float l_FOV = m_NormalMovementFOV;

        if (Input.GetKey(m_RunKeyCode))
        {
            l_Speed = m_Speed * m_FastSpeedMultiplier;
            l_FOV = m_RunMovementFOV;
        }

        m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, l_FOV, m_TransitionStepperFOV);

        l_Direction.Normalize();

        m_Yaw = m_Yaw + l_MouseX * m_YawRotationSpeed * Time.deltaTime * (m_UseYawInverted ? -1.0f : 1.0f);
        m_Pitch = m_Pitch + l_MouseY * m_PitchRotationSpeed * Time.deltaTime * (m_UsePitchInverted ? -1.0f : 1.0f);
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);

        Vector3 l_Movement = l_Direction * l_Speed * Time.deltaTime;

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;

        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_Movement);

        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f)
        {
            m_VerticalSpeed = 0.0f;
        }

        if (m_TimerHopsRemaining > 0)
        {
            m_TimerHopsRemaining -= Time.deltaTime;
        }

        if ((l_CollisionFlags & CollisionFlags.Below) != 0 || m_TimerHopsRemaining <= 0)
        {
            m_VerticalSpeed = 0.0f;
            m_OnGround = true;
        }
        else
        {
            m_OnGround = false;
        }
    }
}
