using System;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {
  public static T Random<T>(this T[] array) {
    return array[UnityEngine.Random.Range(0, array.Length)];
  }

  public static T Random<T>(this List<T> list) {
    return list[UnityEngine.Random.Range(0, list.Count)];
  }
}
