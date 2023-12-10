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

    [Header("Assign - Ranges")]
    public float sightWidth = 20f;
    public float sightHeight = 2f;
    public float hearRange = 30f;

    [Header("Assign - Speeds")]
    public float passiveSpeed = 5f;
    public float alertSpeed = 10f;
    public float aggressiveSpeed = 10f;

    [Header("Assign - Patrol")] public PatrolRoute patrolRoute;

    [Header("No Touch - Info")]
    public bool isPlayerInHearRange;
    public bool isPlayerInSightRange;
    public bool canSeePlayer;
    public Collider[] enemiesInSightRange;
    public Collider[] enemiesInHearRange;

    [Header("No Touch - Info")]
    public Transform playerTransform;
    public NavMeshAgent navMeshAgent;
    public int playerLayer = 1 << 7;
    public int enemyLayer = 1 << 11;
    public Vector3 sightArea;
    public Vector3 sightAreaCenter;

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
        currentState?.ExitState(this);
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
