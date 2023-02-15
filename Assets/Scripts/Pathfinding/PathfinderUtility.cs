using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class PathfinderUtility : MonoBehaviour
{
    [Header("Target Selection OnStart")]
    [SerializeField] private bool _isTargetPlayerOnStart = true;

    private float m_defaultSpeed = 2.0f;               // Reference in case of immobilisation
    private AIPath m_AIPath;                           // Movement, rotation, End Reached Distance, etc.
    private AIDestinationSetter m_AIDestinationSetter; // Target
    private Seeker m_Seeker;                           // Dependency of AIPath


    public readonly int bitmaskConstraintTag = 0;
    private NNConstraint constraint;

    public bool HasReachedEndOfPath => m_AIPath.reachedEndOfPath;

    private void Awake()
    {
        m_AIPath = GetComponent<AIPath>();
        m_AIDestinationSetter = GetComponent<AIDestinationSetter>();
        m_Seeker = GetComponent<Seeker>();
        m_defaultSpeed = m_AIPath.maxSpeed != 0 ? m_AIPath.maxSpeed : m_defaultSpeed;
        SetDefaultNNConstraint();
    }

    private void Start()
    {
        if (_isTargetPlayerOnStart)
        {
            SetTargetAs(Entity_Player.Instance.transform);
        }
    }

    public void EnablePathfinding()
    {
        m_AIPath.enabled = true;
        m_AIDestinationSetter.enabled = true;
        m_Seeker.enabled = true;
    }

    public void DisablePathfinding()
    {
        m_AIPath.enabled = false;
        m_AIDestinationSetter.enabled = false;
        m_Seeker.enabled = false;
    }

    public void SetDefaultNNConstraint()
    {
        // Set Nearest NodeSO Constraint
        constraint = NNConstraint.None;

        // Constrain the search to walkable nodes only
        constraint.constrainWalkability = true;
        constraint.walkable = true;

        // Constrain the search to only nodes with tag 0 (Basic Ground)
        // The 'tags' field is a bitmask
        constraint.constrainTags = true;
        constraint.tags = (1 << bitmaskConstraintTag);
    }

    /// <returns>
    /// Returns true if target was succesfully set
    /// </returns>
    public bool TrySetTargetAs(Transform desiredTarget)
    {
        if (CanReachTarget(desiredTarget))
        {
            SetTargetAs(desiredTarget);
            return true;
        }
        return false;
    }

    public bool CanReachTarget()
    {
        GraphNode toNode = null;
        GraphNode fromNode = null;

        NNConstraint tempConstraint = constraint;
        tempConstraint.walkable = false;
        tempConstraint.constrainWalkability = false;
        toNode = AstarPath.active.graphs[0].GetNearest(m_AIDestinationSetter.target.position, tempConstraint).node;
        fromNode = AstarPath.active.graphs[0].GetNearest(transform.position, constraint).node;

        return PathUtilities.IsPathPossible(fromNode, toNode);
    }

    public bool CanReachTarget(Transform target)
    {
        GraphNode toNode = null;
        GraphNode fromNode = null;

        NNConstraint tempConstraint = constraint;
        tempConstraint.walkable = false;
        tempConstraint.constrainWalkability = false;
        toNode = AstarPath.active.graphs[0].GetNearest(target.position, tempConstraint).node;
        fromNode = AstarPath.active.graphs[0].GetNearest(transform.position, constraint).node;

        return PathUtilities.IsPathPossible(fromNode, toNode);
    }

    public void SetTargetAsPlayer() => m_AIDestinationSetter.target = Entity_Player.Instance.transform;
    public void SetTargetAs(Transform newTarget) => m_AIDestinationSetter.target = newTarget;

    public void SetEndReachedDistance(float newEndReachedDistance) =>  m_AIPath.endReachedDistance = newEndReachedDistance;

    public void SetSpeed(float newSpeed) => m_AIPath.maxSpeed = newSpeed;
    public void SetSpeedAsDefault() => m_AIPath.maxSpeed = m_defaultSpeed;
    public void SetCanMove(bool canMove) => m_AIPath.canMove = canMove;
}
