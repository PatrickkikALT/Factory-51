using System;
using UnityEngine;

public class PlayerReference : MonoBehaviour {
  [SerializeField] private Transform player;
  private void Start() {
    player = GameManager.Instance.player;
  }
}
