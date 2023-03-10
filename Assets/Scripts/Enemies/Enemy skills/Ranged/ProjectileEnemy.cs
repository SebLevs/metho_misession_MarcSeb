using UnityEngine;

public abstract class ProjectileEnemy : BaseCollisionHandler, IPoolable, IPauseListener, IUpdateListener
{
    [Header("Damage")]
    [SerializeField] protected int m_damage;

    [Header("Movement")]
    [SerializeField] protected float m_baseSpeed;
    protected Vector2 m_oldVelocity;

    [Header("Timer")]
    [SerializeField] protected float returnToPoolTime = 10f;
    private SequentialStopwatch _stopwatch;

    protected override void OnAwake()
    {
        base.OnAwake();
        _stopwatch = new SequentialStopwatch(returnToPoolTime, () => ReturnToPool());
    }

    public Transform Target { get; set; }

    public abstract ProjectileEnemy GetFromPool(Transform spawnLocation);
    public abstract void ReturnToPool();

    public void SetTargetAsPlayer()
    {
        Target = Entity_Player.Instance.transform;
    }

    public void SetTargetAs(Transform target)
    {
        Target = target;
    }

    public void ShootTowardsTarget()
    {
        Vector2 direction = (Target.position - transform.position).normalized;
        transform.up = direction;
        m_rigidBody.velocity = direction * m_baseSpeed;
    }

    public void ShootTowards(Vector2 direction)
    {
        transform.up = direction;
        m_rigidBody.velocity = direction * m_baseSpeed;
    }

    public void StopMovement() => m_rigidBody.velocity = Vector2.zero;

    public void ResumeMovement() => m_rigidBody.velocity = m_oldVelocity;

    public void SaveVelocity() => m_oldVelocity = m_rigidBody.velocity;

    public virtual void OnGetFromAvailable() 
    {
        _stopwatch.StartTimer();
    }

    public virtual void OnReturnToAvailable() 
    { 
        StopMovement();
        _stopwatch.Reset(true);
    }

    public void OnEnable()
    {
        GameManager.Instance.SubscribeToPauseGame(this);
        UpdateManager.Instance.SubscribeToUpdate(this);
    }

    public void OnDisable()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.UnSubscribeFromPauseGame(this);
        }

        if (UpdateManager.Instance)
        {
            UpdateManager.Instance.UnSubscribeFromUpdate(this);
        }
    }

    public virtual void OnPauseGame()
    {
        SaveVelocity();
        StopMovement();
    }

    public virtual void OnResumeGame() { ResumeMovement(); }

    public virtual void OnUpdate()
    {
        _stopwatch.OnUpdateTime();
    }
}
