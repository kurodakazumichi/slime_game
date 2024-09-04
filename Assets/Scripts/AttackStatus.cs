using UnityEngine;

public enum Attribute
{
  Nil = 0,      // 設定なし
  Non = 1 << 0, // 無
  Fir = 1 << 1, // 火
  Wat = 1 << 2, // 水
  Thu = 1 << 3, // 雷
  Ice = 1 << 4, // 氷
  Tre = 1 << 5, // 木
  Hol = 1 << 6, // 聖
  Dar = 1 << 7, // 闇
}

public class AttackStatus
{
  public float Power = 0f;
  public Flag32 Attributes = new Flag32();
}
