using UnityEngine;

public abstract class EnemyType : MonoBehaviour
{
    [field: SerializeField] public string TypeName { get; private set; }
    protected Enemy m_context;

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake() { }

    private void Start()
    {
        OnStart();
    }

    public virtual void OnStart() { }

    public abstract void ReturnToPool();

    public void Init(Enemy context)
    {
        m_context = context;
    }
}
