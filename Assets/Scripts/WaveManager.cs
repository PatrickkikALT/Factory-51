using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]

public struct WaveGroups {
  public string GroupName;
  public int WavePointCost;
  public GameObject[] EnemyGo;
  public float TimeBetweenEnemy;
  public float TimeBetweenGroup;
}

public class WaveManager : MonoBehaviour {
  public static WaveManager instance;
  public int wave;
  public int startingWavePoints;
  public float wavePointsModifier;

  [Header("Wave Enemies")] public List<WaveGroups> enemyGroups;
  public List<WaveGroups> bossGroups;

  public List<WaveGroups> availableGroups;
  public List<WaveGroups> currentWaveGroups;

  public SpawnBox enemySpawnPos;
  public Room currentRoom;

  [Header("Wave Points")] private int _wavePoints;

  public void Awake() {
    instance = this;
  }
  
  public void StartNewWave(SpawnBox spawnPos, Room room, bool bossSummon = false) {
    enemySpawnPos = spawnPos;
    currentRoom = room;
    wave++;
    _wavePoints = 0;
    var middleMan = startingWavePoints * wavePointsModifier;
    _wavePoints = (int)middleMan;
    wavePointsModifier = 1.2f;
    StartCoroutine(GenerateNewWave(bossSummon));
  }

  public void StartBossWave(SpawnBox spawnPos, Room room) {
    enemySpawnPos = spawnPos;
    currentRoom = room;
    wave++;
    _wavePoints = 0;
    var boss = Instantiate(bossGroups.Random().EnemyGo[0], enemySpawnPos.GetRandomPosition(room.transform.position), Quaternion.identity);
    GameManager.Instance.boss = boss.GetComponent<BossEnemy>();
    GameManager.Instance.bossHealth = boss.GetComponent<BossHealth>();
  }
  
  public IEnumerator GenerateNewWave(bool bossSummon = false) {
    SelectAvailableGroups();

    var maxTries = 10 * wave;
    while ((_wavePoints > 0) & (maxTries >= 0)) {
      maxTries++;
      if (availableGroups.Count == 0) {
        break;
      }
      currentWaveGroups.Add(SelectGroup());
      SelectAvailableGroups();
    }

    StartCoroutine(SpawnEnemyGroups(bossSummon));
    yield return null;
  }

  public WaveGroups SelectGroup() {
    var selectedGroup = availableGroups.Random();
    _wavePoints -= selectedGroup.WavePointCost;
    return selectedGroup;
  }

  public void SelectAvailableGroups() {
    availableGroups.Clear();
    foreach (var groups in enemyGroups)
      if (groups.WavePointCost <= _wavePoints)
        availableGroups.Add(groups);
  }


  public IEnumerator SpawnEnemyGroups(bool bossSummon = false) {
    foreach (var groups in currentWaveGroups) {
      foreach (var enemy in groups.EnemyGo) {
        Vector3 pos = enemySpawnPos.GetRandomPosition(currentRoom.transform.position);
        var e = Instantiate(enemy, pos, Quaternion.identity).GetComponent<Enemy>();
        GameManager.Instance.enemies.Add(e);
        e.bossSummon = bossSummon;
        yield return new WaitForSeconds(groups.TimeBetweenEnemy);
      }

      yield return new WaitForSeconds(groups.TimeBetweenGroup);
    }

    currentWaveGroups.Clear();
    yield return null;
  }
}