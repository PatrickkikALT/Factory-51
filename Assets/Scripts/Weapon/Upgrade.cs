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
  public bool AddUpgradeAndMoveToManager() {
    var gameManager = GameManager.Instance.gameObject;
    var up = gameManager.AddComponent<Upgrade>();
    up.type = type;
    up.bulletPrefab = bulletPrefab;
    up.damageMultiplier = damageMultiplier;
    var success = UpgradeManager.Instance.AddUpgrade(up);
    Destroy(this);
    return true;
  }

  public abstract void Shoot();
}