using System;
using TMPro;
using UnityEngine;

public class ChestPopUp : MonoBehaviour {
  [Header("References")]
  private TMP_Text _popUpText;
  public LootBox chest;
  private bool _hasOpened;

  private void Start() {
    _popUpText = GameManager.Instance.playerText;
  }

  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.TryGetComponent(out PlayerReference _)) {
      _popUpText.text = "Press E to Open Chest";
    }
  }

  private void OnTriggerStay(Collider other) {
    if (_hasOpened) return;
    if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.TryGetComponent(out PlayerReference _)) {
      if (GameManager.Instance.playerPressedE) {
        _hasOpened = true;
        chest.OpenChest();
        GameManager.Instance.playerPressedE = false;
        Destroy(this);
      }
    }
  }

  private void OnTriggerExit(Collider other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.TryGetComponent(out PlayerReference _)) {
      _popUpText.text = "";
    }
  }
}
