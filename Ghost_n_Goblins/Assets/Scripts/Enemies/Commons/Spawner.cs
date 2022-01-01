using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [Header("Generic settings")]
    [Tooltip("Spawned prefab")]
    [SerializeField] GameObject _EnemyToBeSpawned;
    [Tooltip("Max number of enemies on screen at a given time")]
    [SerializeField] float _MaxEnemyOnScreen;
    [Tooltip("Total number of enemies that will spawn from this spawner")]
    [SerializeField] int _MaxEnemySpawning;
    [Tooltip("Is the spawning infinite ?")]
    [SerializeField] bool _InfiniteSpawning;
    [Tooltip("Is the spawner horizontal ? (Otherwise it's vertical)")]
    [SerializeField] bool _Flying;
    [Tooltip("Range around the player")]
    [SerializeField] float _NearPlayerSpawnRange;
    [Tooltip("Cooldown between spawns")]
    [SerializeField] float _SpawnCooldown;
    [SerializeField] int _DetectionRange; //for vertical spawners


    [Header("Ground unit options")]
    [Tooltip("Spawn near the player ? (Not available for flying units)")]
    [SerializeField] bool _SpawnNearPlayer;
    [Tooltip("Y coordinate to spawn to (Not available for flying units)")]
    [SerializeField] float _ySpawnCoordinate;

    [Header("Spawner's bounds")]
    [Tooltip("Enemies will spawn between start & end")]
    [SerializeField] Transform _SpawnerStart;
    [Tooltip("Enemies will spawn between start & end")]
    [SerializeField] Transform _SpawnerEnd;

    float _ElapsedTime = 0;
    int _EnemySpawnedCount;

    List<GameObject> _OnScreenEnemies = new List<GameObject>();

    private void Update() {
        UpdateList();
        if (_OnScreenEnemies.Count < _MaxEnemyOnScreen && (_EnemySpawnedCount < _MaxEnemySpawning || _InfiniteSpawning) && _ElapsedTime > _SpawnCooldown) {
            if(CheckIfPlayerInBounds()) SpawnEnemy();
            _ElapsedTime = 0;
        }
        _ElapsedTime += Time.deltaTime;
    }

    void UpdateList() {
        foreach (GameObject go in _OnScreenEnemies) if (go == null) _OnScreenEnemies.Remove(go);
    }
    
    void SpawnEnemy() {
        Vector3 position;
        _EnemySpawnedCount++;
        float playerX = LevelInterface.Instance.Player.transform.position.x;
        if (_SpawnNearPlayer) position = new Vector3(Random.Range(playerX - _NearPlayerSpawnRange, playerX + _NearPlayerSpawnRange), _ySpawnCoordinate, 0);
        else {
            if (_Flying) position = new Vector3(_SpawnerStart.position.x, Random.Range(_SpawnerStart.position.y, _SpawnerEnd.position.y), 0);
            else position = new Vector3(Random.Range(_SpawnerStart.position.x, _SpawnerEnd.position.x), _ySpawnCoordinate, 0);
        }
        GameObject go = Instantiate(_EnemyToBeSpawned, position, _EnemyToBeSpawned.transform.rotation);
        _OnScreenEnemies.Add(go);
    }

    bool CheckIfPlayerInBounds() {
        Vector3 playerPosition = LevelInterface.Instance.Player.transform.position;
        if (_Flying) return DetectInRange(_DetectionRange);
        else return playerPosition.x < _SpawnerEnd.position.x && playerPosition.x > _SpawnerStart.position.x;
    }

    bool DetectInRange(float radius) { //todo: move in layers class
        Collider2D collider2D = Physics2D.OverlapCircle(_SpawnerStart.position, radius / 2, Layers.Instance.PlayerLayerMask);
        return collider2D != null;
    }
}
