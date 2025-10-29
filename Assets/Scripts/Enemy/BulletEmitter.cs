using System.Collections;
using UnityEngine;

public abstract class BulletEmitter : MonoBehaviour {
  [Header("Base Emit Settings")] public int amount;

  public int damage;
  public GameObject bullet;
  public float timeToExplode;
  public BulletType type;

  private void OnEnable() {
    StartCoroutine(TimeUntilExplode());
  }

  public abstract void EmitBullets(float startingAngle);

  private IEnumerator TimeUntilExplode() {
    yield return new WaitForSeconds(timeToExplode);
    EmitBullets(0);
    PoolManager.Enqueue(this.gameObject, type);
  }
}