using UnityEngine;

public class Player_ShootState : State<Entity_Player>
{
    public Player_ShootState(Entity_Player context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
        
    }

    public override void OnEnter()
    {
        m_controller.DesiredActions.ConsumeAllActions(PlayerActionsType.SHOOT);
        if(m_controller.canAttack)
        {
            Transform shootFrom = Entity_Player.Instance.shootFrom;
            BulletBehavior bullet = WeaponManager.Instance.bulletPool.GetFromAvailable(shootFrom.position, Quaternion.identity);
            bullet.playerIsShooting = true;
            m_controller.audios.bulletSound.PlayOneShot(m_controller.audios.bulletSound.GetRandomClip());
            bullet.TargetMask = m_controller.Mask;
            Physics2D.IgnoreCollision(bullet.col, Entity_Player.Instance.col); // TODO: Change for layer check instead?
            bullet.ShootBullet(Player_Controller.Instance.normalizedLookDirection, m_controller.BulletSpeed);
            m_controller.canAttack = false;
            m_controller.attackDelay.Reset();
            m_controller.attackDelay.StartTimer();
        }
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        m_controller.StateController.OnTransitionState(m_controller.StateContainer.State_Move);
    }
}
