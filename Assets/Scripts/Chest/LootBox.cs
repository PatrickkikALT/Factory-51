using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LootBox : MonoBehaviour {
  public LootTable lootTable;
  public Transform lootPosition;
  private Animator _animator;

  public float speed;

  public void Start() {
    _animator = GetComponent<Animator>();
  }
  
  public IEnumerator Open() {
    _animator.SetTrigger("OpenChest");
    var loot = LootManager.Instance.GetLoot(lootTable);
    var g = Instantiate(loot.prefab, lootPosition);
    yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
    var target = g.transform.position;
    target.y += 1;
    while (Vector3.Distance(g.transform.position, target) > 0.1f) {
      if (!g) yield break;
      var vec3 = new Vector3(0, 0.1f, 0);
      g.transform.position += vec3 * (speed * Time.deltaTime);
      yield return null;
    }
  }
  public void OpenChest() {
    StartCoroutine(Open());
  }
}