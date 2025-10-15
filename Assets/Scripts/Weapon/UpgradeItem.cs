using System;
using UnityEngine;

public class UpgradeItem : MonoBehaviour {
  public Upgrade upgrade;
  
  public void OnTriggerEnter(Collider other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      GameManager.Instance.playerText.text = "Press E to pick up";
    }
  }

  public void OnTriggerStay(Collider other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      if (GameManager.Instance.playerPressedE) {
        var success = upgrade.AddUpgradeAndMoveToManager();
        GameManager.Instance.playerText.text = "";
        if (success) Destroy(gameObject);
      }
    }
  }

  public void OnTriggerExit(Collider other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      GameManager.Instance.playerText.text = "";
    }
  }

  [ContextMenu("Trigger Animation")]
  public void Animation() {
    GetComponent<Animator>().SetTrigger("Open");
  }
}