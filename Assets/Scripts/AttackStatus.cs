using UnityEngine;

public enum Attribute
{
  Non = 0,      // –³
  Fir = 1 << 1, // ‰Î
  Wat = 1 << 2, // …
  Thu = 1 << 3, // —‹
  Ice = 1 << 4, // •X
  Tre = 1 << 5, // –Ø
  Hol = 1 << 6, // ¹
  Dar = 1 << 7, // ˆÅ
}

public class AttackStatus
{
  public float Power = 0f;
  public Flag32 Attributes = new Flag32();
}
