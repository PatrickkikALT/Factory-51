using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LootBox : MonoBehaviour {
  public Upgrade[] upgrades;
  public List<Upgrade> possibleUpgrades;
  public Upgrade[] options;
  public Image[] uiImages;
  public int optionAmount;
  
  
  public void Open() {
    possibleUpgrades = upgrades.ToList();
    for (int i = 0; i < optionAmount; i++) {
      Upgrade upgrade = possibleUpgrades.Random();
      options[i] = upgrade;
      possibleUpgrades.Remove(upgrade);
    }

    possibleUpgrades = upgrades.ToList();
  }
}
