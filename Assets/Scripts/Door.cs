using UnityEngine;

public class Door : MonoBehaviour {
  [SerializeField] private bool triggered;
  public Transform[] spawnPos;

  private void OnTriggerEnter(Collider other) {
    if (triggered) return;
    
    if (other.gameObject.layer == 7 && !other.TryGetComponent<Bullet>(out _)) {
      print("triggered door, spawning wave");
      triggered = true;
      WaveManager.instance.StartNewWave(spawnPos);
    }
  }
}
