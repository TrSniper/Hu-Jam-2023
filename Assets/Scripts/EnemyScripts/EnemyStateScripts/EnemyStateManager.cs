using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyBaseState currentState;

    public EnemyPassiveState enemyPassiveState = new EnemyPassiveState();
    public EnemyAgressiveState enemyAgressiveState = new EnemyAgressiveState();
    public EnemyAlertState enemyAlertState = new EnemyAlertState();
    public EnemyPanicState enemyPanicState = new EnemyPanicState();
    public EnemyDeadState enemyDeadState = new EnemyDeadState();

    private int groundLayer = 1 << 6;
    private int playerLayer = 1 << 7;

    [Header("Assign")]
    [SerializeField] private float sightWidth = 20f;
    [SerializeField] private float sightHeight = 2f;
    [SerializeField] private float hearRange = 30f;
    [SerializeField] private float attackRange = 7f;
    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 5f;

    [Header("Patrol")]
    public PatrolRoute patrolRoute;

    [Header("No Touch - Info")]
    public NavMeshAgent navMeshAgent;
    [SerializeField] private bool isPlayerInHearRange;
    [SerializeField] private bool isPlayerInSightRange;
    public bool isPlayerInAttackRange;
    public bool didEncounterPlayer;

    private Vector3 sightArea;
    private Vector3 sightAreaCenter;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SwitchState(enemyPassiveState);
    }

    private void Update()
    {
        currentState.OnUpdate(this);

        isPlayerInHearRange = Physics.CheckSphere(transform.position, hearRange, playerLayer);
        isPlayerInSightRange = Physics.CheckBox(sightAreaCenter, sightArea / 2 ,Quaternion.identity, playerLayer);
    }

    public void SwitchState(EnemyBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    private void OnDrawGizmosSelected()
    {
        sightArea = new Vector3(sightWidth, sightHeight, sightWidth);
        sightAreaCenter = transform.position + transform.forward * ((sightWidth - transform.lossyScale.z) / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(sightAreaCenter, sightArea);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearRange);
    }
}
