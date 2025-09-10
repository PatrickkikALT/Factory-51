using System.Collections;
using UnityEngine;

public abstract class BulletEmitter : MonoBehaviour {
  [Header("Base Emit Settings")]
  public int amount;
  public int damage;
  public GameObject bullet;
  public float timeToExplode;
  public abstract void EmitBullets(float startingAngle);
  
  private void Start() {
    StartCoroutine(TimeUntilExplode());
  }
  private IEnumerator TimeUntilExplode() {
    yield return new WaitForSeconds(timeToExplode);
    EmitBullets(0);
    Destroy(gameObject);
  }
}