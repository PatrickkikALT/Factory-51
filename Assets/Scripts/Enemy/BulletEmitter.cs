using UnityEngine;

public abstract class BulletEmitter : MonoBehaviour {
  public int amount;
  public int damage;
  public GameObject bullet;
  public abstract void EmitBullets();
}