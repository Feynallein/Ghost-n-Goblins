using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] GameObject _EnemyToBeSpawned; //the prefab the spawner will spawn
    [SerializeField] float _MaxEnemyOnScreen; // max number of enemies on screen at a given time
    [SerializeField] int _MaxEnemySpawning; //nb of enemy that the spawner will spawn in total
    [SerializeField] bool _InfiniteSpawning; //if there is no limite (so above is useless)
    [SerializeField] Transform _SpawnerStart;
    [SerializeField] Transform _SpawnerEnd; // the spawner will spawn enemy between start & end
    [SerializeField] bool _Flying; // if horizontal spawn (otherwise it's vertical)
    [SerializeField] bool _SpawnNearPlayer; //not available for flying units
    [SerializeField] float _NearPlayerSpawnRange;
    [SerializeField] float _SpawnCooldown;
    [SerializeField] float _ySpawnCoordinate; //not available for flying units

    float _ElapsedTime = 0;
    int _EnemySpawnedCount;

    List<GameObject> _OnScreenEnemies = new List<GameObject>();

    private void Update() {
        UpdateList();
        if (_OnScreenEnemies.Count < _MaxEnemyOnScreen && (_EnemySpawnedCount < _MaxEnemySpawning || _InfiniteSpawning) && _ElapsedTime > _SpawnCooldown) {
            if(CheckIfNearPlayer()) SpawnEnemy();
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

    bool CheckIfNearPlayer() {
        Vector3 playerPosition = LevelInterface.Instance.Player.transform.position;
        return _SpawnNearPlayer && !_Flying && playerPosition.x < _SpawnerEnd.position.x && playerPosition.x > _SpawnerStart.position.x;
    }
}
