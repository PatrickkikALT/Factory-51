using Unity.VisualScripting;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour {
  public UpgradeType type;
  public GameObject bulletPrefab;
  public float damageMultiplier;

  protected Transform Player;
  protected Weapon WeaponS;
  protected Transform Weapon;
  
  private void Start() {
    Player = GameManager.Instance.player;
    WeaponS = Player.FindWeapon();
    Weapon = WeaponS.transform;

  }
  //holy this is so ugly i need to rewrite this upgrade manager
  public virtual bool AddUpgradeAndMoveToManager() {
    var gameManager = GameManager.Instance.gameObject;
    var upg = (Upgrade)gameManager.AddComponent(GetType());
    upg.type = type;
    upg.bulletPrefab = bulletPrefab;
    upg.damageMultiplier = damageMultiplier;
    var success = UpgradeManager.Instance.AddUpgrade(upg);
    Destroy(this);
    return success;
  }

  public abstract void Shoot();

  public abstract void OnAdd();
}