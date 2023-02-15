using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable, IFrameUpdateListener, IPauseListener
{
    public EnemyType Type { get; private set; }

    [Header("Health bar")]
    [SerializeField] private ViewHealthBarWithCounter m_healthBar;
    public Health Health { get; private set; }

    protected Rigidbody2D m_rigidbody;
    protected Collider2D m_collider;

    protected SpriteRenderer m_spriteRenderer;
    public Animator Animator { get; protected set; }

    protected EnemyStateController m_stateController;
    public PathfinderUtility PathfinderUtility { get; private set; }

    [field: SerializeField] public int collisionDamage { get; private set; }
    [field: SerializeField] public int TempDamage { get; private set; }

    private void Awake()
    {
        Type = GetComponent<EnemyType>();

        Health = GetComponent<Health>();

        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();

        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Animator = GetComponent<Animator>();

        m_stateController = GetComponent<EnemyStateController>();

        PathfinderUtility = GetComponent<PathfinderUtility>();
    }

    private void Start()
    {
        Init();
    }

    public void OnGetFromAvailable()
    {
        Init();
        EnemyManager.Instance.CurrentlyActiveEnemies.Add(this);
    }

    public void OnReturnToAvailable()
    {
        EnemyManager.Instance.CurrentlyActiveEnemies.Remove(this);
    }

    public void ReturnToPool() => Type.ReturnToPool(this);

    private void Init()
    {
        m_rigidbody.velocity = Vector2.zero;
        Health.FullHeal();
    }

    public void Kill()
    {
        Health.OnInstantDeath();
    }

    public void DelegateSetHealthBarFilling()
    {
        m_healthBar.Filler.SetFilling(Health.Normalized);
        //m_healthBar.Counter.Element.text = Health.CurrentHP.ToString();
    }

    public void OnUpdate()
    {
        // TODO: Call state controller here
        m_stateController.OnUpdate();
        float angle = MathAngleUtilities.GetSignedAngle2D(Entity_Player.Instance.transform, this.transform);
        int index = MathAngleUtilities.GetAngleAsIndex2D_Quad(angle);
        MathAngleUtilities.FlipLocalScale2D(m_spriteRenderer.transform, angle);
        Animator.SetFloat("angle", index);
    }

    public void OnDisable()
    {
        if (UpdateManager.Instance)
        {
            UpdateManager.Instance.UnSubscribeFromUpdate(this);
        }
        if(GameManager.Instance)
        {
            GameManager.Instance.UnSubscribeFromPauseGame(this);
        }
    }

    public void OnEnable()
    {
        UpdateManager.Instance.SubscribeToUpdate(this);
        GameManager.Instance.SubscribeToPauseGame(this);
    }

    public void OnPauseGame()
    {
        Animator.speed = 0f;

        PathfinderUtility.DisablePathfinding();

        m_rigidbody.velocity = Vector2.zero;
        m_collider.enabled = false;
    }

    public void OnResumeGame()
    {
        Animator.speed = 1f;

        m_collider.enabled = true;

        PathfinderUtility.EnablePathfinding();
    }

    public void OnFirstHitPopupHealthBar()
    {
        if (!Health.IsDead && !m_healthBar.gameObject.activeSelf)
        {
            m_healthBar.OnShowQuick();
        }
    }
}
