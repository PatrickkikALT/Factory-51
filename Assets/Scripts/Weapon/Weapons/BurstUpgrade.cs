using System.Collections;
using UnityEngine;

public class BurstUpgrade : Upgrade {

  public float burstDelay;
  public int burstAmount;
  public override void Shoot() {
    StartCoroutine(Burst());
  }

  public IEnumerator Burst() {
    for (int i = 0; i < burstAmount; i++) {
      Bullet bullet = Instantiate(bulletPrefab, Weapon.position + Player.forward, Player.rotation).GetComponent<Bullet>();
      bullet.direction = Player.transform.forward;
      bullet.gameObject.layer = Player.gameObject.layer;
      yield return new WaitForSeconds(burstDelay);
    }
  }
}
