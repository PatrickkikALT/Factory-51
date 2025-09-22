using System.Collections.Generic;
using UnityEngine;

public static class Extensions {
  public static T Random<T>(this T[] array) {
    return array[UnityEngine.Random.Range(0, array.Length)];
  }

  public static T Random<T>(this List<T> list) {
    return list[UnityEngine.Random.Range(0, list.Count)];
  }

  public static Weapon FindWeapon(this Transform transform) {
    return GameManager.Instance.playerWeapon;
  }

  public static Vector3 GetRandomPosition(this BoxCollider bounds) {
    var center = bounds.center - bounds.gameObject.transform.position;

    var minX = center.x - bounds.size.x / 2f;
    var maxX = center.x + bounds.size.x / 2f;

    var minZ = center.z - bounds.size.z / 2f;
    var maxZ = center.z + bounds.size.z / 2f;

    var randomX = UnityEngine.Random.Range(minX, maxX);
    var randomZ = UnityEngine.Random.Range(minZ, maxZ);

    var randomPosition = new Vector3(randomX, 1, randomZ);

    return randomPosition;
  }
}