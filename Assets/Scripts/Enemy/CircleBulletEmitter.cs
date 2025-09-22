using UnityEngine;

public class CircleBulletEmitter : BulletEmitter {
  [ContextMenu("Emit")]
  public override void EmitBullets(float startingAngle) {
    //amount is defined in our base class BulletEmitter
    var angleStep = 360f / amount;
    var angle = 0f;

    for (var i = 0; i < amount; i++) {
      var rot = Quaternion.Euler(0f, 0f, angle);
      var b = Instantiate(bullet, transform.position, rot).GetComponent<Bullet>();
      var rad = angle * Mathf.Deg2Rad;
      var dirForward = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));
      b.direction = dirForward;
      b.gameObject.layer = gameObject.layer;
      angle += angleStep;
    }
  }
}