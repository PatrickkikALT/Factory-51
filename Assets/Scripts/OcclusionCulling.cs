using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OcclusionCulling : MonoBehaviour {
  [Header("Occlusion Settings")] public Vector3 boxCenter = Vector3.zero;
  public Vector3 boxSize = Vector3.one;
  public bool cullingEnabled;
  private Collider[] _overlappingColliders;
  public int timeBetweenCull;
  private Renderer[] _allObjects;

  private void Start() {
    _allObjects = FindObjectsByType<Renderer>(FindObjectsSortMode.None);

    StartCoroutine(Cull());
  }


  private IEnumerator Cull() {
    while (cullingEnabled) {
      _overlappingColliders = Physics.OverlapBox(transform.position + boxCenter, boxSize / 2);
      var renderers = _overlappingColliders.Select(x => x.GetComponent<Renderer>())
                                                      .Where(x => x != null).ToList();
      var outside = _allObjects.Where(x => !renderers.Contains(x)).ToList();
      print($"{outside.Count} renderers culled.");
      outside.ForEach(x => x.enabled = false);

      yield return new WaitForSeconds(timeBetweenCull);
    }
  }

  void OnDrawGizmosSelected() {
    Gizmos.color = Color.green;
    Gizmos.matrix = Matrix4x4.TRS(transform.position + boxCenter, transform.rotation, Vector3.one);
    Gizmos.DrawWireCube(Vector3.zero, boxSize);
  }
}