﻿using System.Collections.Generic;
using System;

public static class Util
{
  public static void ForEach<T>(T[] array, Action<T, int> func)
  {
    int count = array.Length;

    for (int i = 0; i < count; ++i) {
      func(array[i], i);
    }
  }

  public static void ForEach<T>(T[] array, Func<T, int, bool> func)
  {
    int count = array.Length;
    for (int i = 0; i < count; ++i) {
      bool completed = func(array[i], i);
      if (completed) break;
    }
  }

  public static void ForEach<T1, T2>(Dictionary<T1, T2> dic, Action<T1, T2> func)
  {
    foreach (KeyValuePair<T1, T2> data in dic) {
      func(data.Key, data.Value);
    }
  }

  public static void ForEach<T1, T2>(Dictionary<T1, T2> dic, Func<T1, T2, bool> func)
  {
    foreach (KeyValuePair<T1, T2> data in dic) {
      bool completed = func(data.Key, data.Value);
      if (completed) break;
    }
  }

  /// <summary>
  /// リストの中からランダムに１つ取得する
  /// </summary>
  public static T GetRandom<T>(List<T> list)
  {
    return list[UnityEngine.Random.Range(0, list.Count)];
  }

  /// <summary>
  /// 配列の指定したindexの要素を取得を試みる
  /// </summary>
  public static T TryGet<T>(T[] array, int index, T def = default)
  {
    if (0 <= index && index < array.Length) {
      return array[index];
    }

    return default;
  }

  /// <summary>
  /// List型のTryGet
  /// </summary>
  public static T TryGet<T>(List<T> list, int index) where T : class
  {
    if (0 <= index && index < list.Count) {
      return list[index];
    }

    return null;
  }

  /// <summary>
  /// 幸あれ！と祈ると指定した確率でtrueが返ってくる、幸がなければfalse
  /// rateは0~1で指定
  /// </summary>
  public static bool GoodLuck(float rate)
  {
    return (UnityEngine.Random.Range(0, 1f) <= rate);
  }
}