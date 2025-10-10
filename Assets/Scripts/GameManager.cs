using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public static GameManager Instance;
  public Transform player;
  public Weapon playerWeapon;
  public Transform bodyBone;
  public List<Enemy> enemies = new();
  public bool playerPressedE;
  public TMP_Text playerText;
  public Room currentRoom;
  private void Awake() {
    if (Instance is null) Instance = this;
    else Destroy(this);
  }

  public void SetRoom(Room room) {
    if (currentRoom == room) return;
    currentRoom = room;
    WaveManager.instance.StartNewWave(currentRoom.enemySpawnLocations, currentRoom);
    print($"Spawning Wave in room {currentRoom.name}");
  }

  // private void Start() {
  //   Application.targetFrameRate = 60;
  // }
}