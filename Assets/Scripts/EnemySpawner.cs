using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData[] _enemies;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _delay = 1f;

    private GameManager _gameManager;
    private BattleController _battleController;
    private Unit _current;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _battleController = FindObjectOfType<BattleController>();

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            while (_current != null ? _current.isAlive : false)
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(_delay);

            SpawnEnemy();

            yield return new WaitForEndOfFrame();
        }
    }

    private void SpawnEnemy()
    {
        if (_enemies.Length == 0 || _spawnPoint == null) return;

        // Choose an enemy based on spawn chances
        EnemyData enemy = GetRandomEnemy();
        if (enemy != null)
        {
            _current = _gameManager.Spawn(enemy, _spawnPoint.position, _spawnPoint.rotation);

            _battleController.SetEnemy(_current);
        }
    }

    private EnemyData GetRandomEnemy()
    {
        float totalChance = 0f;
        foreach (var enemy in _enemies)
        {
            totalChance += enemy.spawnChance;
        }

        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (var enemy in _enemies)
        {
            cumulativeChance += enemy.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                return enemy;
            }
        }

        return null;
    }
}
