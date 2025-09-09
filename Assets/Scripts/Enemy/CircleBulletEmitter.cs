using System;
using System.Collections;
using UnityEngine;

public class CircleBulletEmitter : BulletEmitter {
  private void Start() {
    StartCoroutine(TimeUntilExplode());
  }

  [ContextMenu("Emit")]
  public override void EmitBullets(float startingAngle) {
    //amount is defined in our base class BulletEmitter
    float angleStep = 360f / amount;
    float angle = 0f;
    
    for (int i = 0; i < amount; i++) {
      Quaternion rot = Quaternion.Euler(0f, 0f, angle);
      GameObject bullet = Instantiate(base.bullet, transform.position, rot);
      float rad = angle * Mathf.Deg2Rad;
      Vector3 dirForward = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));
      bullet.GetComponent<Bullet>().direction = dirForward;
      angle += angleStep;
    }
  }

  public IEnumerator TimeUntilExplode() {
    yield return new WaitForSeconds(timeToExplode);
    EmitBullets(0);
    Destroy(gameObject);
  }
}