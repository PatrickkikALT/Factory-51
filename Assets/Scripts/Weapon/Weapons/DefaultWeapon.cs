public class DefaultWeapon : Upgrade {
  public override void Shoot() {
    var bullet = Instantiate(bulletPrefab, WeaponS.shootPos.position, Player.rotation).GetComponent<Bullet>();
    bullet.direction = GameManager.Instance.bodyBone.transform.forward;
    bullet.gameObject.layer = Player.gameObject.layer;
  }
}
