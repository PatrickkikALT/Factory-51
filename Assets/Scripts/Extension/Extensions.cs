using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions {
  public static T Random<T>(this T[] array) => array[UnityEngine.Random.Range(0, array.Length)];

  public static T Random<T>(this List<T> list) => list[UnityEngine.Random.Range(0, list.Count)];

  public static Weapon FindWeapon(this Transform transform) => GameManager.Instance.playerWeapon;

  public static int Random(this int i, int max) => UnityEngine.Random.Range(0, max);

  public static float Lerp(this float f, float to, float speed) =>  f * (1 - speed) + to * (1 - speed);

  public static T RandomEnum<T>(this Enum _) where T : Enum => ((T[])Enum.GetValues(typeof(T))).Random();

  public static Loot Random(this LootTable loot) => loot.loot.Random();
  public static Vector3 Random(this BoxCollider bounds) {
    var center = bounds.center - bounds.gameObject.transform.position;

    var size = bounds.size;
    var minX = center.x - size.x / 2f;
    var maxX = center.x + size.x / 2f;

    var minZ = center.z - size.z / 2f;
    var maxZ = center.z + size.z / 2f;

    var randomX = UnityEngine.Random.Range(minX, maxX);
    var randomZ = UnityEngine.Random.Range(minZ, maxZ);

    var randomPosition = new Vector3(randomX, 1, randomZ);

    return randomPosition;
  }
}