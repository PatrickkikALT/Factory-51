using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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
    if (currentRoom.bossRoom) {
      WaveManager.instance.StartBossWave(currentRoom.enemySpawnLocations, currentRoom);
    }
    else {
      WaveManager.instance.StartNewWave(currentRoom.enemySpawnLocations, currentRoom);
    }
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