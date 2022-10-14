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
    NavMeshAgent m_NavMeshAgent;
    public List<Transform> m_PatrolTargets;
    int m_CurrentPatrolTardetId = 0;
    public float m_HearRangeDistance;
    
    public float m_VisualConeAngle = 60.0f;
    public float m_SightDistance = 8.0f;
    public LayerMask m_SightLayerMask;
    public float m_EyesHeight = 1.8f;
    public float m_EyesPlayerHeight = 1.8f;

    void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        SetIdleState();
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
        m_NavMeshAgent.destination = m_PatrolTargets[m_CurrentPatrolTardetId].position;
    }

    void UpdatePatrolState()
    {
        if (PatrolTargetPositionArrived())
            MoveToNextPatrolPosition();
        if (HearsPlayer())
            SetAlertState();
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
    }

    void UpdateAlertState()
    {

    }

    void SetChaseState()
    {
        m_State = TState.CHASE;
    }

    void UpdateChaseState()
    {

    }

    void SetAttackState()
    {
        m_State = TState.ATTACK;
    }

    void UpdateAttackState()
    {

    }

    void SetHitState()
    {
        m_State = TState.HIT;
    }

    void UpdateHitState()
    {

    }

    void SetDieState()
    {
        m_State = TState.DIE;
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
        Debug.Log("Hit life " + Life);
    }
}