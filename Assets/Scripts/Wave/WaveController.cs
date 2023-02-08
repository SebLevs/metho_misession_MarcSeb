using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class WaveController
{
    [Header("Timers")]
    [SerializeField] private float _endOfWaveTime;
    [Range(1, 10)][SerializeField] private float _tickRange;
    private SequentialStopwatch _spawnerStopWatch;
    private SequentialStopwatch _nextWaveStopWatch;

    [Header("Factory")]
    [SerializeField] private AbstractEnemyFactory _enemyFactory;
    [Min(1)][SerializeField] private int _lowQuantitySpawns;
    [Min(1)][SerializeField] private int _highQuantitySpawns;

    public void Init(Action callback = null)
    {
        _spawnerStopWatch = new SequentialStopwatch(GetRandomTimeInRange());
        _nextWaveStopWatch = new SequentialStopwatch(_endOfWaveTime, callback);
    }

    /// <summary>
    /// </summary>
    /// <returns>Has reached next wave time</returns>
    public bool Tick()
    {
        _spawnerStopWatch.OnUpdateTime();
        _nextWaveStopWatch.OnUpdateTime();

        if (_spawnerStopWatch.HasReachedTarget())
        {
            _spawnerStopWatch.Reset(true);
            _spawnerStopWatch.SetNewTarget(GetRandomTimeInRange());
            CreateEnemyWave();
            _spawnerStopWatch.StartTimer();
        }

        if (_nextWaveStopWatch.HasReachedTarget())
        {
            return true;
        }
        return false;
    }

    private float GetRandomTimeInRange()
    {
        return UnityEngine.Random.Range(0, _tickRange);
    }

    private void CreateEnemyWave()
    {
        for (int i = 0; i < _lowQuantitySpawns; i++)
        {
            _enemyFactory.CreateLowQuantityEnemy(WaveManager.Instance.GetRandomSpawnPoint());
        }
        for (int i = 0; i < _lowQuantitySpawns; i++)
        {
            _enemyFactory.CreateHighQuantityEnemy(WaveManager.Instance.GetRandomSpawnPoint());
        }
    }
}
