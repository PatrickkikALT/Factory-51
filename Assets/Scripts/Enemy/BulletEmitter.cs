using UnityEngine;

public abstract class BulletEmitter : MonoBehaviour {
  [Header("Emit Settings")]
  public int amount;
  public int damage;
  public GameObject bullet;
  public int timeToExplode;
  public abstract void EmitBullets(float startingAngle);
  
}