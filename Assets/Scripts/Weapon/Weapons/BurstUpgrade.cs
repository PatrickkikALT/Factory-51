using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BurstUpgrade : Upgrade {
  public float burstDelay;
  public int burstAmount;

  public override void Shoot() {
    StartCoroutine(Burst());
  }
  public IEnumerator Burst() {
    for (var i = 0; i < burstAmount; i++) {
      GameObject obj = null;
      if (PoolManager.TryDequeue(BulletType.PLAYER, out obj)) {
        obj.transform.position = WeaponS.shootPos.position;
        obj.transform.rotation = Player.rotation;
      }
      else {
        obj = Instantiate(bulletPrefab, WeaponS.shootPos.position, Player.rotation);
      }

      var bullet = obj.GetComponent<Bullet>();
      bullet.direction = GameManager.Instance.bodyBone.transform.forward;
      bullet.gameObject.layer = Player.gameObject.layer;
      bullet.type = BulletType.PLAYER;
      yield return new WaitForSeconds(burstDelay);
    }
  }

  public override bool AddUpgradeAndMoveToManager() {
    var gameManager = GameManager.Instance.gameObject;
    var upg = (BurstUpgrade)gameManager.AddComponent(GetType());
    upg.type = type;
    upg.bulletPrefab = bulletPrefab;
    upg.damageMultiplier = damageMultiplier;
    upg.burstAmount = burstAmount;
    upg.burstDelay = burstDelay;
    var success = UpgradeManager.Instance.AddUpgrade(upg);
    Destroy(this);
    return success;
  }

  public override void OnAdd() {
    GameManager.Instance.playerWeapon.delay += (burstDelay * burstAmount);
  }
}