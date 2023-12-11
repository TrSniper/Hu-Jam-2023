using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyBaseState currentState;
    public EnemyPassiveState enemyPassiveState = new EnemyPassiveState();
    public EnemyAggressiveState enemyAggressiveState = new EnemyAggressiveState();
    public EnemyAlertState enemyAlertState = new EnemyAlertState();
    public EnemyPanicState enemyPanicState = new EnemyPanicState();
    public EnemyDeadState enemyDeadState = new EnemyDeadState();

    [Header("Brave enemies doesn't hide in AlertState, they continue to patrol")] public bool isBrave;
    [Header("Assign")] public int health = 10;

    [Header("Assign - Ranges")]
    public float sightWidth = 20f;
    public float sightHeight = 2f;
    public float hearRange = 30f;

    [Header("Assign - Speeds")]
    public float passiveSpeed = 5f;
    public float alertSpeed = 10f;

    [Header("Assign - Patrol and Strategic Positions")]
    public PatrolRoute patrolRoute;
    public StrategicPositions strategicPositions;

    [Header("No Touch - Info")]
    public bool isAttacking;
    public bool isChasing;

    [Header("No Touch - Info")]
    public bool isPlayerInHearRange;
    public bool isPlayerInSightRange;
    public bool canSeePlayer;
    public Collider[] enemiesInSightRange;
    public Collider[] enemiesInHearRange;

    [Header("No Touch - Info")]
    public Transform playerTransform;
    public EnemyAnimationManager eam;
    public NavMeshAgent navMeshAgent;
    public int playerLayer = 1 << 7;
    public int enemyLayer = 1 << 11;
    public Vector3 sightArea;
    public Vector3 sightAreaCenter;

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        eam = GetComponent<EnemyAnimationManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        ChangeState(enemyPassiveState);
    }

    private void Update()
    {
        currentState.OnUpdate(this);
        Debug.Log(currentState);
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

    //TODO: better
    public void GetDamage(int damageTaken)
    {
        health -= damageTaken;
    }
}
