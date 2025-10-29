using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
  public static GameManager Instance;
  public Transform player;
  public Weapon playerWeapon;
  public Transform bodyBone;
  public List<Enemy> enemies = new();
  public bool playerPressedE;
  public TMP_Text playerText;
  public Room currentRoom;
  private Movement _movement;
  public GameObject hud;
  public GameObject deathScreen;
  public GameObject winScreen;
  public PlayerHealth playerHealth;
  public GameObject pauseMenu;

  public Image gear;
  
  public TMP_Text roomText;
  public DungeonGenerator generator;
  private void Awake() {
    Instance = this;
  }

  private void Start() {
    _movement = player.GetComponent<Movement>();
    _movement.canMove = true;
    roomText.text = $"{0}/{generator.dungeonSize}";
  }

  public void EndGame(bool win) {
    _movement.canMove = false;
    Cursor.lockState = CursorLockMode.None;
    if (win) {
      winScreen.SetActive(true);
      hud.SetActive(false);
    }
    else {
      hud.SetActive(false);
      deathScreen.SetActive(true);
    }
  }
  public void SetRoom(Room room) {
    if (currentRoom == room) return;
    currentRoom = room;
    roomText.text = $"{room.id}/{generator.dungeonSize}";
    StartCoroutine(RotateGear());
    if (currentRoom.bossRoom) {
      ClearEnemies();
      WaveManager.instance.StartBossWave(currentRoom.enemySpawnLocations, currentRoom);
    }
    else {
      WaveManager.instance.StartNewWave(currentRoom.enemySpawnLocations, currentRoom);
    }
  }

  private IEnumerator RotateGear() {
    for (int i = 0; i < 360; i++) {
      gear.transform.Rotate(new Vector3(0, 0, 1));
      yield return null;
    }
  }

  private void ClearEnemies() {
    enemies.ForEach(Destroy);
  }

  public void TogglePauseMenu() {
    Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
    _movement.canMove = !_movement.canMove;
    pauseMenu.SetActive(!pauseMenu.activeSelf);
  }

  // private void Start() {
  //   Application.targetFrameRate = 60;
  // }
  
  
}