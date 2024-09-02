using UnityEngine;

public enum Attribute
{
  Non = 0,      // ��
  Fir = 1 << 1, // ��
  Wat = 1 << 2, // ��
  Thu = 1 << 3, // ��
  Ice = 1 << 4, // �X
  Tre = 1 << 5, // ��
  Hol = 1 << 6, // ��
  Dar = 1 << 7, // ��
}

public class AttackStatus
{
  public float Power = 0f;
  public Flag32 Attributes = new Flag32();
}
