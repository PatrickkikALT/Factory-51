using System;
using System.Collections;
using UnityEngine;

public class CircleBulletEmitter : BulletEmitter {


  [ContextMenu("Emit")]
  public override void EmitBullets(float startingAngle) {
    //amount is defined in our base class BulletEmitter
    float angleStep = 360f / amount;
    float angle = 0f;
    
    for (int i = 0; i < amount; i++) {
      Quaternion rot = Quaternion.Euler(0f, 0f, angle);
      Bullet b = Instantiate(bullet, transform.position, rot).GetComponent<Bullet>();
      float rad = angle * Mathf.Deg2Rad;
      Vector3 dirForward = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));
      b.direction = dirForward;
      b.gameObject.layer = gameObject.layer;
      angle += angleStep;
    }
  }


}