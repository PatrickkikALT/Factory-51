using UnityEngine;

public class DefaultWeapon : Upgrade {
  public override void Shoot() {
    GameObject obj;
    if (PoolManager.TryDequeue(out obj)) {
      obj.transform.position = WeaponS.shootPos.position;
      obj.transform.rotation = Player.rotation;
    }
    else {
      obj = Instantiate(bulletPrefab, WeaponS.shootPos.position, Player.rotation);
    }

    var bullet = obj.GetComponent<Bullet>();
    bullet.direction = GameManager.Instance.bodyBone.transform.forward;
    bullet.gameObject.layer = Player.gameObject.layer;
  }

  public override void OnAdd() {
    return;
  }
}
