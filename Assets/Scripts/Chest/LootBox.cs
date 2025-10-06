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

  public void Start() {
    _animator = GetComponent<Animator>();
  }
  
  public IEnumerator Open() {
    _animator.SetTrigger("OpenChest");
    var info = _animator.GetCurrentAnimatorStateInfo(0);
    //get length of state so animation plays before the loot gets spawned
    yield return new WaitForSeconds(info.length * 2);
    var loot = LootManager.Instance.GetLoot(lootTable);
    var g = Instantiate(loot.prefab, lootPosition);
    g.GetComponent<Animator>().SetTrigger("Open");
  }
  //wrapper method cuz you cant start coroutines with contextmenu :(((
  #if UNITY_EDITOR
  [ContextMenu("Open")]
  public void OpenChest() {
    StartCoroutine(Open());
  }
  #endif
}