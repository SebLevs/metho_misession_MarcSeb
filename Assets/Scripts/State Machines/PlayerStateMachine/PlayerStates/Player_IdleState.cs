using UnityEngine;

public class Player_IdleState : State<Entity_Player>
{
    public Player_IdleState(Entity_Player context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
        
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if(Player_Controller.Instance.moveDirection != Vector2.zero) 
        {
            m_controller.StateController.OnTransitionState(m_controller.StateContainer.State_Move);
        }
        if (Entity_Player.Instance.Health.IsDead)
        {
            m_controller.StateController.OnTransitionState(m_controller.StateContainer.State_Dead);
        }
        if (m_controller.DesiredActions.Contains(PlayerActionsType.SHOOT))
        {
            m_controller.StateController.OnTransitionState(m_controller.StateContainer.State_Shoot);
        }
        if (m_controller.DesiredActions.Contains(PlayerActionsType.SPECIALSHOOT))
        {
            m_controller.StateController.OnTransitionState(m_controller.StateContainer.State_SpecialShoot);
        }
        if (m_controller.DesiredActions.Contains(PlayerActionsType.DODGE))
        {
            m_controller.StateController.OnTransitionState(m_controller.StateContainer.State_Dodge);
        }
    }
}
