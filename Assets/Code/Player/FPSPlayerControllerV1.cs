using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;

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
    public CharacterController m_CharacterController;
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
    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;
    public KeyCode m_ShootingGalleryCode = KeyCode.L;
    public KeyCode m_Pause = KeyCode.P;
    public KeyCode m_Restart = KeyCode.R;
    bool m_AngleLocked = false;
    bool m_AimLocked = true;

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

    [Space(0.5f)]
    [Header("Shoot")]
    [Space(1f)]
    public float m_MaxShootDistance = 50.0f;
    public LayerMask m_ShootLayerMask;
    public GameObject m_Bullet;
    public GameObject m_Prefab;
    public int m_MaxAmmo;
    public float m_MaxShield;
    private float m_CurrentShield;
    private int m_CurrentAmmo;
    private PoolOfElements m_PoolOfElements;

    [Header("Animations")]
    public Animation m_Animation;
    public AnimationClip m_IdleAnimationClip;
    public AnimationClip m_ShootAnimationClip;
    public AnimationClip m_ReloadAnimationClip;

    float m_Life = 5.0f;
    Vector3 m_InitialPosition;
    Quaternion m_InitialRotation;

    private ShootingGallery m_Gallery;
    private float m_Time;
    private float m_MaxTime = 30;
    private bool m_TimerActive = false;
    public bool m_Paused;
    public bool m_Dead;

    public static event Action<bool> OnPause;
    public static event Action<bool> OnDie;

    bool m_Shooting = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Life = GameController.GetGameController().GetPlayerLife();
        GameController.GetGameController().SetPlayer(this);

        m_Yaw = transform.rotation.y;
        m_Pitch = m_PitchController.localRotation.x;

        m_ElapsedTimeInTheAir = m_MinTimeInTheAir;

        Cursor.lockState = CursorLockMode.Locked;
        m_AimLocked = Cursor.lockState == CursorLockMode.Locked;

        SetFOVIfParametersAreEmpty();
        m_CurrentAmmo = m_MaxAmmo;
        m_CurrentShield = m_MaxShield;

        SetIdleWeaponAnimation();

        m_InitialPosition = transform.position;
        m_InitialRotation = transform.rotation;

        //m_PoolOfElements = new PoolOfElements(25, m_Prefab);
        m_Time = m_MaxTime;
        
        m_Paused = false;
        m_Dead = false;
    }

    public void SetNewRespawnPosition(Vector3 Position, Quaternion Rotation)
    {
        m_InitialPosition = Position;
        m_InitialRotation = Rotation;
    }

    private void SetFOVIfParametersAreEmpty()
    {
        if (m_GeneralCamera && m_WeaponCamera && m_NormalMovementFOV == 0 || m_RunMovementFOV == 0)
        {
            m_WeaponCamera.fieldOfView = m_GeneralCamera.fieldOfView;

            m_NormalMovementFOV = m_NormalMovementFOV == 0 ? m_GeneralCamera.fieldOfView : m_NormalMovementFOV;
            m_RunMovementFOV = m_NormalMovementFOV + m_SumRateRunningFOV;
        }
    }

#if UNITY_EDITOR
    void UpdateInputDebug()
    {
        if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
            m_AngleLocked = !m_AngleLocked;
        if (Input.GetKeyDown(m_DebugLockKeyCode))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        }
    }
#endif

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        UpdateInputDebug();
#endif

        Vector3 l_RightDirection = transform.right;
        Vector3 l_ForwardDirection = transform.forward;
        Vector3 l_Direction = Vector3.zero;

        float l_Speed = m_Speed;

        float l_MouseX = Input.GetAxis("Mouse X"); // It is the old Unity input
        float l_MouseY = Input.GetAxis("Mouse Y");

#if UNITY_EDITOR
        if (m_AngleLocked)
        {
            l_MouseX = 0.0f;
            l_MouseY = 0.0f;
        }
#endif

        //Shoot
        if (Input.GetMouseButtonDown(0) && IsShooting())
            Shoot();

        // Movement
        if (Input.GetKey(m_UpKeyCode)) l_Direction = l_ForwardDirection;
        if (Input.GetKey(m_DownKeyCode)) l_Direction = -l_ForwardDirection;
        if (Input.GetKey(m_RightKeyCode)) l_Direction += l_RightDirection;
        if (Input.GetKey(m_LeftKeyCode)) l_Direction -= l_RightDirection;

        // Jump
        if (Input.GetKeyDown(m_JumpKeyCode) && m_OnGround || Input.GetKeyDown(m_JumpKeyCode) && m_CanJump)
        {
            m_VerticalSpeed = m_JumpSpeed;
            m_CanJump = false;
        }

        //ShootingGallery
        if(Input.GetKeyDown(m_ShootingGalleryCode))
        {
            ShootingGallery.GetShootingGallery().ActivateShootingGallery();
            m_TimerActive = true;
        }

        if(m_Time <= 0)
        {
            m_TimerActive=false;
        }

        if(m_TimerActive)
        {
            m_Time -= Time.deltaTime;
        }


        //Pause
        if(Input.GetKeyDown(m_Pause))
        {
            Pause();
        }

        //Restart
        if(Input.GetKeyDown(m_Restart))
        {
            ShootingGallery.GetShootingGallery().RestartGallery();
        }

        // FOV control
        float l_FOV = m_NormalMovementFOV;

        if (Input.GetKey(m_RunKeyCode))
        {
            l_Speed = m_Speed * m_FastSpeedMultiplier;
            l_FOV = m_RunMovementFOV;
        }

        m_GeneralCamera.fieldOfView = Mathf.Lerp(m_GeneralCamera.fieldOfView, l_FOV, m_TransitionStepperFOV);
        m_WeaponCamera.fieldOfView = Mathf.Lerp(m_WeaponCamera.fieldOfView, l_FOV, m_TransitionStepperFOV);

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

        if ((l_CollisionFlags & CollisionFlags.Below) != 0)
        {
            m_VerticalSpeed = 0.0f;
            m_OnGround = true;
            m_ElapsedTimeInTheAir = m_MinTimeInTheAir;
        }
        else
        {
            m_OnGround = false;
        }

        if (m_ElapsedTimeInTheAir > 0)
        {
            m_ElapsedTimeInTheAir -= Time.deltaTime;
            if (m_VerticalSpeed < m_MinHeightSpeedTolerance || m_VerticalSpeed > m_MaxHeightSpeedTolerance) m_ElapsedTimeInTheAir = 0.0f;
            if (m_VerticalSpeed > m_MinHeightSpeedTolerance && m_VerticalSpeed < m_MaxHeightSpeedTolerance) m_CanJump = true;
            else m_CanJump = false;
        }
        else
        {
            m_CanJump = false;
        }
    }

    bool IsShooting()
    {
        return !m_Shooting;
    }

    bool CanShoot()
    {
        if (m_CurrentAmmo > 0)
        {
            return true;
        }
        else
        {
            Recharge();
            return false;
        }
    }

    void Shoot()
    {
        //SetShootWeaponAnimation();
        Ray l_Ray = m_WeaponCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit l_RaycastHit;
        if (CanShoot())
        {
            if (Physics.Raycast(l_Ray, out l_RaycastHit, m_MaxShootDistance, m_ShootLayerMask.value))
            {
                CreateShootHitParticles(l_RaycastHit.collider, l_RaycastHit.point, l_RaycastHit.normal);
                m_CurrentAmmo -= 1;
            }
        }
        SetShootWeaponAnimation();
        m_Shooting = true;
    }

    void Recharge()
    {
        if(m_CurrentAmmo <= 0)
        {
            m_CurrentAmmo = m_MaxAmmo;
        }
    }

    public void ResetTime()
    {
        m_Time = m_MaxTime;
    }

    void CreateShootHitParticles(Collider _collider, Vector3 Position, Vector3 Normal)
    {
        //Debug.DrawRay(Position, Normal * 5.0f, Color.red, 2.0f);
        //m_Bullet = Instantiate(m_Prefab, Position, Quaternion.LookRotation(Normal)) as GameObject;

        //GameObject l_Decal = m_PoolOfElements.GetNextElement();
        //Debug.Log("QUIEN ERE? " + l_Decal.name);
        //l_Decal.SetActive(true);
        //l_Decal.transform.position = Position;
        //l_Decal.transform.rotation = Quaternion.LookRotation(Normal);

        GameObject l_Decal = ObjectPool.m_SharedInstance.GetPooledObject();
        if (l_Decal != null)
        {
            l_Decal.transform.position = Position;
            l_Decal.transform.rotation = Quaternion.LookRotation(Normal);
            l_Decal.SetActive(true);
            StartCoroutine(DestroyOnTime(l_Decal)); 
        }
    }

    public float GetTime()
    {
        return m_Time;
    }

    public int GetAmmo()
    {
        return m_CurrentAmmo;
    }

    public void AddAmmo(int Ammo)
    {
        if (m_CurrentAmmo < 100)
        {
            m_CurrentAmmo += Ammo;
            if(m_CurrentAmmo > 100) { m_CurrentAmmo = m_MaxAmmo; }
        }
    }

    public float GetShield()
    {
        return m_CurrentShield;
    }

    public void AddShield(float Shield)
    {
        if(m_CurrentShield < 100)
        {
            m_CurrentShield += Shield;
            if(m_CurrentShield > 100) { m_CurrentShield = m_MaxShield; }
        }
    }

    void SetIdleWeaponAnimation()
    {
        m_Animation.CrossFade(m_IdleAnimationClip.name);
    }

    void SetShootWeaponAnimation()
    {
        m_Animation.CrossFade(m_ShootAnimationClip.name, 0.1f);
        m_Animation.CrossFadeQueued(m_IdleAnimationClip.name, 0.1f);
        StartCoroutine(EndShoot());
    }

    public float GetLife()
    {
        return m_Life;
    }

    public void AddLife(float Life)
    {
        m_Life = Mathf.Clamp(m_Life + Life, 0.0f, 1.0f);
    }

    public void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "Item")
        {
            Other.GetComponent<Item>().Pick(this);
            Debug.Log("item");
        }
        if (Other.tag == "DeadZone")
        {
            Kill();
        }
        if (Other.tag == "Respawn")
        {
            SetNewRespawnPosition(Other.transform.position, Other.transform.rotation);
        }
    }

    void Kill()
    {
        m_Life = 0;
        //GameController.GetGameController().RestartGame();
        Die();
        //m_Dead = true;
    }

    public void RestartGame()
    {
        m_Life = 100;
        m_CurrentShield = 100;
        m_CharacterController.enabled = false;
        transform.position = m_InitialPosition;
        transform.rotation = m_InitialRotation;
        m_CharacterController.enabled = true;
        m_Dead = false;
    }

    public void RecieveDamage(float Damage)
    {
        if (m_CurrentShield >= 0)
        {
            m_Life -= Damage * 0.25f;
            m_CurrentShield -= Damage * 0.75f;
        }
        else
        {
            m_CurrentShield = 0;
            m_Life -= Damage;
        }

        if (m_Life <= 0)
        {
            Kill();
        }
    }

    public void Pause()
    {
        if (m_Paused)
        {
            m_Paused = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            m_AimLocked = false;
        }
        else
        {
            m_Paused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            m_AimLocked = true;
        }
        OnPause?.Invoke(m_Paused);
    }

    public void Die()
    {
        Debug.Log("died");
        if (m_Dead)
        {
            m_Dead = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            m_AimLocked = false;
        }
        else
        {
            m_Dead = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            m_AimLocked = true;
        }
        OnDie?.Invoke(m_Dead);
    }

    IEnumerator DestroyOnTime(GameObject Decal)
    {
        yield return new WaitForSeconds(2);
        Decal.SetActive(false);
        Decal.transform.parent = null;
    }

    IEnumerator EndShoot()
    {
        yield return new WaitForSeconds(m_ShootAnimationClip.length);
        m_Shooting = false;
    }
}
