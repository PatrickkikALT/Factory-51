using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LootBox : MonoBehaviour {
  public Upgrade[] upgrades;
  public List<Upgrade> possibleUpgrades;
  public List<Upgrade> options = new();
  public TMP_Text[] uiImages;
  public int optionAmount;

  private void OnTriggerEnter(Collider other) {
    Open();
  }


  public void Open() {
    possibleUpgrades = upgrades.ToList();
    for (var i = 0; i < optionAmount; i++) {
      if (possibleUpgrades.Count <= 0) break;
      var upgrade = possibleUpgrades.Random();
      print(upgrade);
      options.Add(upgrade);
      possibleUpgrades.Remove(upgrade);
    }

    possibleUpgrades = upgrades.ToList();
  }
}