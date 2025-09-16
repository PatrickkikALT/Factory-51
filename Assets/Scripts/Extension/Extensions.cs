using System.Collections.Generic;
using UnityEngine;

public static class Extensions {
  public static T Random<T>(this T[] array) 
    => array[UnityEngine.Random.Range(0, array.Length)];

  public static T Random<T>(this List<T> list)
    => list[UnityEngine.Random.Range(0, list.Count)];

  public static Weapon FindWeapon(this Transform transform) {
    return GameManager.Instance.playerWeapon;
  }
}