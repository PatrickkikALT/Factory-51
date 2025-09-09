using UnityEngine;

public class SquareBulletEmitter : BulletEmitter {

  public float squareSize = 2f;

  [ContextMenu("Emit")]
  public override void EmitBullets() {
    Vector3[] corners = new Vector3[4];
    float halfSize = squareSize * 0.5f;
    
    corners[0] = new Vector3(-halfSize, 0f, -halfSize);
    corners[1] = new Vector3(halfSize, 0f, -halfSize);
    corners[2] = new Vector3(halfSize, 0f, halfSize);
    corners[3] = new Vector3(-halfSize, 0f, halfSize);

    for (int i = 0; i < corners.Length; i++) {
      Vector3 start = corners[i];
      Vector3 end = corners[(i + 1) % 4];

      for (int j = 0; j < amount; j++) {
        float t = j / (float)(amount - 1);
        Vector3 offset = Vector3.Lerp(start, end, t);

        GameObject bullet = Instantiate(base.bullet, transform.position + offset, Quaternion.identity);

        bullet.GetComponent<Bullet>().direction = offset;
      }
    }

    Destroy(gameObject);
  }
}