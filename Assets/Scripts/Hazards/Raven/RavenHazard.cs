using UnityEngine;

public class RavenGoosePatrol
{
    public bool IsGoosePatrolling;
    public int GoosePatrolCount;
    public Transform GoosePatrolPointA;
    public Transform GoosePatrolPointB;
}

public class RavenHazard : MonoBehaviour
{
    [Header("Raven Properties")]
    [SerializeField] private float speed;
    [SerializeField, Range(0f, 1f)] private float timeReductionPercentage = 0.2f;
    
    [Header("Main Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    
    [Header("Geese Hazard Integration")]
    [SerializeField, Tooltip("Can be empty if no geese!")] private Goose relevantGoose;
    [SerializeField] private int goosePatrolMaxCount;
    
    private Rigidbody2D _rb2d;
    private Transform _currentTarget;
    private readonly RavenGoosePatrol _goosePatrol = new();
    
    // for now just debug log when in radius
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ReduceTime(timeReductionPercentage);
        }
    }
    
    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        pointA.SetParent(null);
        pointB.SetParent(null);
    }
    
    private void Start()
    {
        transform.position = pointA.position;
        _currentTarget = pointB;
        if (!relevantGoose) return;
        _goosePatrol.GoosePatrolPointA = relevantGoose.GetPointA();
        _goosePatrol.GoosePatrolPointB = relevantGoose.GetPointB();
    }

    private void OnEnable()
    {
        if (!relevantGoose) return;
        relevantGoose.OnCallRaven += MoveToGoose;
    }

    private void OnDisable()
    {
        if (!relevantGoose) return;
        relevantGoose.OnCallRaven -= MoveToGoose;
    }

    private void MoveToGoose()
    {
        _currentTarget = _goosePatrol.GoosePatrolPointA;
        _goosePatrol.IsGoosePatrolling = true;
        _goosePatrol.GoosePatrolCount = 0;
    }

    private void FixedUpdate()
    {
        var dir = (_currentTarget.position - transform.position).normalized;
        // _rb2d.MovePosition(dir * (speed * Time.fixedDeltaTime) + transform.position);
        _rb2d.linearVelocity = dir * speed;

        if (_goosePatrol.IsGoosePatrolling)
        {
            if (_goosePatrol.GoosePatrolCount > goosePatrolMaxCount)
            {
                _goosePatrol.IsGoosePatrolling = false;
                return;
            }
            if (!(Vector2.Distance(transform.position, _currentTarget.position) < 0.1f)) return;
            _currentTarget = _currentTarget == _goosePatrol.GoosePatrolPointA ?
                _goosePatrol.GoosePatrolPointB : _goosePatrol.GoosePatrolPointA;
            _goosePatrol.GoosePatrolCount++;
            return;
        }
        
        if (Vector2.Distance(transform.position, _currentTarget.position) < 0.1f)
            _currentTarget = _currentTarget == pointA ? pointB : pointA;
    }
    
    private void OnDrawGizmos()
    {
        if (!pointA || !pointB) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pointA.position, pointB.position);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointA.position, 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointB.position, 0.5f);
    }
}
