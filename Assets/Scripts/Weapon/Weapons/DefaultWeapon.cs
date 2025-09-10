using System;
using UnityEngine;

public class DefaultWeapon : Upgrade {
  public override void Shoot() {
    Bullet bullet = Instantiate(bulletPrefab, Player.position + Player.forward, Player.rotation).GetComponent<Bullet>();
    bullet.direction = Player.forward;
    bullet.gameObject.layer = Player.gameObject.layer;
  }
}
