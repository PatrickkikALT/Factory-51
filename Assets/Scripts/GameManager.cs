using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public static GameManager Instance;
  public Transform player;
  public Weapon playerWeapon;

  public List<Enemy> enemies = new();

  private void Awake() {
    if (Instance is null) Instance = this;
    else Destroy(this);
  }
}