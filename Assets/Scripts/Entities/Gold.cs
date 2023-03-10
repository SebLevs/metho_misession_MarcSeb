using UnityEngine;

public class Gold : MonoBehaviour
{
    public int minGoldValue;
    public int manGoldValue;
    [HideInInspector] public int goldOnDeath;
    private Entity_Player player;

    private void Awake()
    {
        goldOnDeath = Random.Range(minGoldValue, manGoldValue + 1);
    }

    private void Start()
    {
        player = Entity_Player.Instance;
    }

    public void GiveGoldToPLayerOnDeath()
    {
        if (player.currentGold + goldOnDeath >= player.MaxGold)
        {
            player.currentGold = player.MaxGold;
        }
        else
        {
            player.currentGold += goldOnDeath;
        }

        player.RefreshGoldBar();
    }
}