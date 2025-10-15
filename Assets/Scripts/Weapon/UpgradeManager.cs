using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
  public static UpgradeManager Instance;
  public List<Upgrade> equippedUpgrades = new();
  public Weapon playersWeapon;
  public Upgrade defaultUpgrade;


  private void Awake() {
    if (Instance is null) Instance = this;
    else Destroy(this);
  }

  public void Start() {
    playersWeapon = GameManager.Instance.playerWeapon;
    AddUpgrade(defaultUpgrade);
  }

  public bool AddUpgrade(Upgrade upgrade) {
    switch (upgrade.type) {
      case UpgradeType.UNIQUE:
        if (equippedUpgrades.Count(upgrade1 => upgrade1.type == UpgradeType.UNIQUE) != 0) {
          var upg = equippedUpgrades.First(up => up.type == UpgradeType.UNIQUE);
          equippedUpgrades.Remove(upg);
          playersWeapon.ShootEvent -= upg.Shoot;
        }

        break;
      case UpgradeType.ADDITIVE:
        break;
      default:
        Debug.LogError($"No upgrade type found for upgrade {upgrade.name}");
        return false;
    }
    
    equippedUpgrades.Add(upgrade);
    playersWeapon.AddUpgrade(upgrade);
    playersWeapon.currentDamageMultiplier = upgrade.damageMultiplier;
    upgrade.OnAdd();
    return true;
  }
}