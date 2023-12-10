using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public bool deadStart;

    public EnemyBaseState currentState;
    public EnemyPassiveState enemyPassiveState = new EnemyPassiveState();
    public EnemyAggressiveState enemyAggressiveState = new EnemyAggressiveState();
    public EnemyAlertState enemyAlertState = new EnemyAlertState();
    public EnemyPanicState enemyPanicState = new EnemyPanicState();
    public EnemyDeadState enemyDeadState = new EnemyDeadState();

    [Header("Assign")]
    public float sightWidth = 20f;
    public float sightHeight = 2f;
    public float hearRange = 30f;
    public float attackRange = 7f;
    public float walkingSpeed = 2f;
    public float runningSpeed = 5f;

    [Header("Assign - Patrol")] public PatrolRoute patrolRoute;

    [Header("No Touch - Info")]
    public bool isPlayerInHearRange;
    public bool isPlayerInSightRange;
    public bool canSeePlayer;
    public bool isPlayerInAttackRange;
    public bool didEncounterPlayer;
    public Collider[] enemiesInSightRange;
    public Collider[] enemiesInHearRange;

    [Header("No Touch - Info")]
    public Vector3 sightArea;
    public Vector3 sightAreaCenter;

    [Header("No Touch - Info")]
    public Transform playerTransform;
    public NavMeshAgent navMeshAgent;
    public int playerLayer = 1 << 7;
    public int enemyLayer = 1 << 11;

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (deadStart) ChangeState(enemyDeadState);
        else ChangeState(enemyPassiveState);
    }

    private void Update()
    {
        currentState.OnUpdate(this);
    }

    public void ChangeState(EnemyBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(sightAreaCenter, sightArea);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearRange);
    }
}
