using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Loot {
  public GameObject prefab;
  public Upgrade upgrade;
  public Rarity rarity;
}

[CreateAssetMenu(menuName = "Loot Table", fileName = "New Loot Table")]
public class LootTable : ScriptableObject {
  public Loot[] loot;
}
