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

  public HealthSliderHelper bossHealthSlider;
  public BossHealth bossHealth;
  public BossEnemy boss;
  
  private void Awake() {
    Instance = this;
  }

  private void Start() {
    
    var rr = Screen.currentResolution.refreshRateRatio;
    int targetFPS = Mathf.RoundToInt((float)rr.numerator / rr.denominator);
    Application.targetFrameRate = targetFPS;
    QualitySettings.vSyncCount = 0;
    
    _movement = player.GetComponent<Movement>();
    _movement.canMove = true;
    roomText.text = $"{0}/{generator.dungeonSize}";
    // Invoke("LateStart", 0.1f);
  }

  // private void LateStart() {
  //   foreach (GameObject r in generator.roomsInstance) {
  //     r.SetActive(false);
  //   }
  //   SetRoom(generator.roomsInstance[0].GetComponent<Room>());
  // }

  public void EndGame(bool win) {
    _movement.canMove = false;
    playerWeapon.canShoot = false;
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
    // generator.roomsInstance[room.id].SetActive(true);
    // if (room.id != generator.roomsInstance.Count - 1) {
    //   generator.roomsInstance[room.id + 1].SetActive(true);
    // }
    // if (room.id != 0) generator.roomsInstance[room.id - 1].SetActive(true);
    // else if (room.id == 0) return;
    StartCoroutine(RotateGear());
    if (room.bossRoom) {
      ClearEnemies();
      bossHealthSlider.gameObject.SetActive(true);
      WaveManager.instance.StartBossWave(currentRoom.enemySpawnLocations, room);
    }
    else {
      WaveManager.instance.StartNewWave(currentRoom.enemySpawnLocations, room, false);
    }

    room.disabled = true;
  }

  private IEnumerator RotateGear() {
    for (int i = 0; i < 360; i++) {
      gear.transform.Rotate(new Vector3(0, 0, 1));
      yield return null;
    }
  }

  private void ClearEnemies() {
    foreach (Enemy e in enemies) {
      Destroy(e.gameObject);
    }
  }

  public void TogglePauseMenu() {
    Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
    _movement.canMove = !_movement.canMove;
    playerWeapon.canShoot = !playerWeapon.canShoot;
    pauseMenu.SetActive(!pauseMenu.activeSelf);
  }

  // private void Start() {
  //   Application.targetFrameRate = 60;
  // }
  
  
}