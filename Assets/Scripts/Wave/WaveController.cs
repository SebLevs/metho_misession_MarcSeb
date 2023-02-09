using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    private int _currentWaveIndex = 0;
    [SerializeField] private EnemyWave[] m_waves;

    // TODO: Make generic observer pattern to call a random position from this list
    [SerializeField] private List<PositionGetter2D> _enemySpawnPoints;

    private void Awake()
    {
        SetEnemyWaves();
    }

    private void Update()
    {
        if (_currentWaveIndex < m_waves.Length) 
        {
            m_waves[_currentWaveIndex].Tick();
        }
    }

    public Vector2 GetRandomSpawnPoint()
    {
        int index = Random.Range(0, _enemySpawnPoints.Count);

        return _enemySpawnPoints[index].GetRandomPosition();
    }

    private void SetEnemyWaves()
    {
        for (int i = 0; i < m_waves.Length; i++)
        {
            m_waves[i].Init(this, waveEndsCallback: () => _currentWaveIndex++);
        }
    }
}
