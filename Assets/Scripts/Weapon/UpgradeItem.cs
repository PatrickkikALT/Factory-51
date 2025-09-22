using UnityEngine;

public class UpgradeItem : MonoBehaviour {
  public Upgrade upgrade;

  public void OnTriggerEnter(Collider other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      var success = UpgradeManager.Instance.AddUpgrade(upgrade);
      if (success) Destroy(gameObject);
    }
  }
}