using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Entity : MonoBehaviour
{
    public enum TState
    {
        IDLE,
        PATROL,
        ALERT,
        CHASE,
        ATTACK,
        HIT,
        DIE
    }

    public TState m_State;
    TState m_PreviousState;

    NavMeshAgent m_NavMeshAgent;
    EntityHealth m_EntityHealth;
    public List<Transform> m_PatrolTargets;
    int m_CurrentPatrolTardetId = 0;
    public float m_HearRangeDistance = 5;
    
    public float m_VisualConeAngle = 60.0f;
    public float m_SightDistance = 8.0f;
    public LayerMask m_SightLayerMask;
    public float m_EyesHeight = 1.8f;
    public float m_EyesPlayerHeight = 1.8f;

    float m_Angle = 360.0f;
    public float m_RotationTime = 1.0f;
    Vector3 m_Axis = Vector3.up;
    float m_RotationLeft;

    public float m_DistanceToShoot = 8;

    public Animation m_AnimationEntity;
    public AnimationClip m_AnimationEntityIdle;
    public AnimationClip m_AnimationEntityPatrol;
    public AnimationClip m_AnimationEntityHit;
    public AnimationClip m_AnimationEntityDie;

    bool m_BeingHit = false;
    bool m_BeingDie = false;

    bool m_Shooting = false;

    public float m_TimeToDisappear = 2;

    public float m_GunDamage = 25;

    public List<GameObject> m_ItemDrop;

    void Awake()
    {
        m_EntityHealth = GetComponent<EntityHealth>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        SetIdleState();

        SetAnimationEntityIdle();
        //m_ItemDrop = new List<GameObject>(3);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case TState.IDLE:
                UpdateIdleState();
                break;
            case TState.PATROL:
                UpdatePatrolState();
                break;
            case TState.ALERT:
                UpdateAlertState();
                break;
            case TState.CHASE:
                UpdateChaseState();
                break;
            case TState.ATTACK:
                UpdateAttackState();
                break;
            case TState.HIT:
                UpdateHitState();
                break;
            case TState.DIE:
                UpdateDieState();
                break;
        }

        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        Vector3 l_EyesPosition = transform.position + Vector3.up * m_EyesHeight;
        Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_EyesPlayerHeight;
        Debug.DrawLine(l_EyesPosition, l_PlayerEyesPosition, SeesPlayer() ? Color.red : Color.blue);
    }

    void SetIdleState()
    {
        m_State = TState.IDLE;
        SetPatrolState();
    }

    void UpdateIdleState()
    {

    }

    void SetPatrolState()
    {
        m_State = TState.PATROL;
        m_NavMeshAgent.isStopped = false;
        m_NavMeshAgent.destination = m_PatrolTargets[m_CurrentPatrolTardetId].position;
    }

    void UpdatePatrolState()
    {
        if (PatrolTargetPositionArrived())
            MoveToNextPatrolPosition();
        if (HearsPlayer())
            SetAlertState();
        if (SeesPlayer())
            SetChaseState();
    }

    bool PatrolTargetPositionArrived()
    {
        return !m_NavMeshAgent.hasPath && !m_NavMeshAgent.pathPending && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
    }

    void MoveToNextPatrolPosition()
    {
        ++m_CurrentPatrolTardetId;
        if (m_CurrentPatrolTardetId >= m_PatrolTargets.Count)
            m_CurrentPatrolTardetId = 0;
        m_NavMeshAgent.destination = m_PatrolTargets[m_CurrentPatrolTardetId].position;
    }

    bool SeesPlayer()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        Vector3 l_DirectionToPlayerXZ = l_PlayerPosition - transform.position;
        l_DirectionToPlayerXZ.y = 0.0f;
        l_DirectionToPlayerXZ.Normalize();

        Vector3 l_ForwardXZ = transform.forward;
        l_ForwardXZ.y = 0.0f;
        l_ForwardXZ.Normalize();

        Vector3 l_EyesPosition = transform.position + Vector3.up * m_EyesHeight;
        Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_EyesPlayerHeight;
        Vector3 l_Direction = l_PlayerEyesPosition - l_EyesPosition;

        float l_Lenght = l_Direction.magnitude;
        l_Direction /= l_Lenght;

        Ray l_Ray = new Ray(l_EyesPosition, l_Direction);

        return Vector3.Distance(l_PlayerPosition, transform.position) < m_VisualConeAngle &&
               Vector3.Dot(l_ForwardXZ, l_DirectionToPlayerXZ) > Mathf.Cos(m_VisualConeAngle * Mathf.Deg2Rad / 2.0f) &&
               !Physics.Raycast(l_Ray, l_Lenght, m_SightLayerMask.value);
    }

    void SetAlertState()
    {
        m_State = TState.ALERT;
        m_NavMeshAgent.isStopped = true;
        StartCoroutine(EndOfRotation());
    }

    void UpdateAlertState()
    {
        transform.RotateAround(transform.position, m_Axis, m_Angle * Time.deltaTime / m_RotationTime);

        if (SeesPlayer())
            SetChaseState();
    }

    IEnumerator EndOfRotation()
    {
        yield return new WaitForSeconds(m_RotationTime);
        SetPatrolState();
    }

    void SetChaseState()
    {
        m_State = TState.CHASE;
        m_NavMeshAgent.isStopped = false;
    }

    void UpdateChaseState()
    {
        if (SeesPlayer())
        {
            m_NavMeshAgent.destination = GameController.GetGameController().GetPlayer().transform.position;

            if (InDistanceToShoot())
            {
                SetAttackState();
            }
        }
        else
        {
            SetAlertState();
        }
    }

    bool InDistanceToShoot()
    {
        return (transform.position - GameController.GetGameController().GetPlayer().transform.position).magnitude <= m_DistanceToShoot;
    }

    void SetAttackState()
    {
        m_State = TState.ATTACK;
        m_NavMeshAgent.isStopped = true;
    }

    void UpdateAttackState()
    {
        if (SeesPlayer() && InDistanceToShoot())
        {
            if (!m_Shooting)
                Shoot();
        }
        else
        {
            SetChaseState();
        }
    }

    void Shoot()
    {
        m_Shooting = true;
        StartCoroutine(Reloading());
        GameController.GetGameController().GetPlayer().RecieveDamage(m_GunDamage);
    }

    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(m_AnimationEntityDie.length + m_TimeToDisappear);

        m_Shooting = false;
    }

    void SetHitState(float Life)
    {
        m_PreviousState = TState.ALERT;
        m_State = TState.HIT;
        m_EntityHealth.m_Health -= Life;
        SetAnimationEntityHit();
    }

    void UpdateHitState()
    {
            
    }

    void SetDieState(float Life)
    {
        m_State = TState.DIE;
        m_EntityHealth.m_Health -= Life;
        SetAnimationEntityDie();
    }

    void UpdateDieState()
    {
            
    }

    bool HearsPlayer()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        return Vector3.Distance(l_PlayerPosition, transform.position) <= m_HearRangeDistance;
    }

    public void Hit(float Life)
    {
        m_NavMeshAgent.isStopped = true;

        if (m_EntityHealth.m_Health - Life > 0) {
            if (!m_BeingHit)
                SetHitState(Life);
        }
        else
        {
            if (!m_BeingDie)
                SetDieState(Life);
        }
    }

    void SetAnimationEntityIdle()
    {
        m_AnimationEntity.CrossFade(m_AnimationEntityIdle.name);
    }

    void SetAnimationEntityHit()
    {
        m_BeingHit = true;

        m_AnimationEntity.CrossFade(m_AnimationEntityHit.name, 0.1f);
        m_AnimationEntity.CrossFadeQueued(m_AnimationEntityIdle.name, 0.1f);

        StartCoroutine(EndHit());
    }

    void SetAnimationEntityDie()
    {
        m_BeingDie = true;

        m_AnimationEntity.CrossFade(m_AnimationEntityDie.name);

        StartCoroutine(DropItemOnDie());

        StartCoroutine(EndDie());
    }

    IEnumerator EndHit()
    {
        yield return new WaitForSeconds(m_AnimationEntityHit.length);

        m_BeingHit = false;
        m_State = m_PreviousState;
    }

    IEnumerator DropItemOnDie()
    {
        yield return new WaitForSeconds(m_AnimationEntityDie.length);
      
    }

    IEnumerator EndDie()
    {
        yield return new WaitForSeconds(m_AnimationEntityDie.length);

        gameObject.SetActive(false);
        int rand = Random.RandomRange(0, 2);
            Instantiate(m_ItemDrop[rand], new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), Quaternion.identity);
    }
}