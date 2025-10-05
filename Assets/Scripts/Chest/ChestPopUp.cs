using System;
using TMPro;
using UnityEngine;

public class ChestPopUp : MonoBehaviour {
  [Header("References")]
  private TMP_Text _popUpText;
  public LootBox chest;

  private void Start() {
    _popUpText = GameManager.Instance.playerText;
  }

  private void OnCollisionEnter(Collision other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      _popUpText.text = "Press E to Open Chest";
    }
  }

  private void OnCollisionStay(Collision other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      if (GameManager.Instance.playerPressedE) {
        chest.OpenChest();
      }
    }
  }

  private void OnCollisionExit(Collision other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      _popUpText.text = "";
    }
  }
}
