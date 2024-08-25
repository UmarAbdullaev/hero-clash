using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerData _player;
    [SerializeField] private Transform _spawnPoint;

    private GameManager _gameManager;
    private BattleController _battleController;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _battleController = FindObjectOfType<BattleController>();

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        _battleController.SetPlayer(
            _gameManager.Spawn(_player, _spawnPoint.position, _spawnPoint.rotation)
        );
    }
}
