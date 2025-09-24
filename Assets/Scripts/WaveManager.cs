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

  public List<WaveGroups> availableGroups;
  public List<WaveGroups> currentWaveGroups;

  public Transform[] enemySpawnPos;

  [Header("Wave Points")] private int _wavePoints;

  public void Awake() {
    instance = this;
  }

  [ContextMenu("StartNewWave")]
  public void StartNewWave(Transform[] spawnPos) {
    enemySpawnPos = spawnPos;
    wave++;
    _wavePoints = 0;
    var middleMan = startingWavePoints * wavePointsModifier;
    _wavePoints = (int)middleMan;
    wavePointsModifier = 1.2f;
    StartCoroutine(GenerateNewWave());
  }

  public IEnumerator GenerateNewWave() {
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

    StartCoroutine(SpawnEnemyGroups());
    yield return null;
  }

  public WaveGroups SelectGroup() {
    var selectedGroup = availableGroups.Random();
    print(_wavePoints);
    print(selectedGroup.WavePointCost);
    _wavePoints -= selectedGroup.WavePointCost;
    return selectedGroup;
  }

  public void SelectAvailableGroups() {
    availableGroups.Clear();
    foreach (var groups in enemyGroups)
      if (groups.WavePointCost <= _wavePoints)
        availableGroups.Add(groups);
  }


  public IEnumerator SpawnEnemyGroups() {
    foreach (var groups in currentWaveGroups) {
      print(groups.GroupName);
      foreach (var enemy in groups.EnemyGo) {
        Transform pos = enemySpawnPos.Random();
        GameManager.Instance.enemies.Add(Instantiate(enemy, pos.position, pos.rotation)
          .GetComponent<Enemy>());
        yield return new WaitForSeconds(groups.TimeBetweenEnemy);
      }

      yield return new WaitForSeconds(groups.TimeBetweenGroup);
    }

    currentWaveGroups.Clear();
    yield return null;
  }
}