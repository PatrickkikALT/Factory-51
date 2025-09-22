using UnityEngine;

public class SquareBulletEmitter : BulletEmitter {
  [Header("Unique Settings")] public float squareSize = 2f;

  [ContextMenu("Emit")]
  public override void EmitBullets(float startingAngle) {
    var corners = new Vector3[4];
    var halfSize = squareSize * 0.5f;

    corners[0] = new Vector3(-halfSize, 0f, -halfSize);
    corners[1] = new Vector3(halfSize, 0f, -halfSize);
    corners[2] = new Vector3(halfSize, 0f, halfSize);
    corners[3] = new Vector3(-halfSize, 0f, halfSize);

    for (var i = 0; i < corners.Length; i++) {
      var start = corners[i];
      var end = corners[(i + 1) % 4];

      for (var j = 0; j < amount; j++) {
        var t = j / (float)(amount - 1);
        var offset = Vector3.Lerp(start, end, t);

        var b = Instantiate(bullet, transform.position + offset, Quaternion.identity).GetComponent<Bullet>();
        b.direction = offset;
        b.gameObject.layer = gameObject.layer;
      }
    }

    Destroy(gameObject);
  }
}