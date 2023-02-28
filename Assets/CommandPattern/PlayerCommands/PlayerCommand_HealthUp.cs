using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommand_HealthUp : ICommand
{
    public PlayerCommand_HealthUp()
    {
    }

    public void Execute()
    {
        if (Entity_Player.Instance.Health.MaxHP <= 1000)
        {
            Entity_Player.Instance.Health.SetMaxHP(Entity_Player.Instance.Health.MaxHP + 20);
            Entity_Player.Instance.RefreshHealthBar();
        }
    }

    public void UnExecute()
    {
        if (Entity_Player.Instance.Health.MaxHP >= 300)
        {
            Entity_Player.Instance.Health.SetMaxHP(Entity_Player.Instance.Health.MaxHP - 20);
            Entity_Player.Instance.RefreshHealthBar();
        }
    }
}