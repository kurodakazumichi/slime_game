using System;

public static class MyEnum
{
  /// <summary>
  /// Enum�̒����烉���_���Ȓl���擾
  /// </summary>
  public static T Random<T>() where T : Enum
  {
    var count = Enum.GetNames(typeof(T)).Length;
    var index = UnityEngine.Random.Range(0, count);
    var value = Enum.GetValues(typeof(T)).GetValue(index);
    return (T)value;
  }

  /// <summary>
  /// �񋓌^�̗v�f�̐��������[�v����
  /// </summary>
  /// <param name="action">�R�[���o�b�N(value)</param>
  public static void ForEach<T>(Action<T> func) where T : Enum
  {
    foreach (T value in Enum.GetValues(typeof(T))) {
      func(value);
    }
  }

  /// <summary>
  /// �񋓌^�̗v�f�̐��������[�v����
  /// </summary>
  /// <param name="action">�R�[���o�b�N(value, int)</param>
  public static void ForEach<T>(Action<T, int> func) where T : Enum
  {
    int index = 0;
    foreach (T value in Enum.GetValues(typeof(T))) {
      func(value, index++);
    }
  }

  /// <summary>
  /// �񋓌^�̗v�f�̐��������[�v����
  /// </summary>
  /// <param name="func">func(value, key)</param>
  public static void ForEach<T>(Action<T, string> func) where T : Enum
  {
    foreach (T value in Enum.GetValues(typeof(T))) {
      func(value, Enum.GetName(typeof(T), value));
    }
  }

  /// <summary>
  /// Enum�v�f�̕�����z����擾
  /// </summary>
  public static string[] GetNames<T>() where T : Enum
  {
    return Enum.GetNames(typeof(T));
  }

  /// <summary>
  /// �������Enum�ɋ����ϊ�����
  /// </summary>
  public static T Parse<T>(string name) where T : Enum
  {
    return (T)Enum.Parse(typeof(T), name);
  }

  /// <summary>
  /// �����񂩂�Enum�̎擾�����݂�
  /// </summary>
  public static bool TryParse<T>(string name, out T result) where T : struct
  {
    return Enum.TryParse(name, out result);
  }
}