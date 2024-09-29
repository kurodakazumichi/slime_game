namespace MyGame.Master
{
  /// <summary>
  /// ReadOnlyのSkillEntityインターフェース
  /// </summary>
  public interface ISkillEntity
  {
    SkillId Id { get; }

    int MaxExp { get; }
    float FirstRecastTime { get; }
    float LastRecastTime { get; }
    int FirstPower { get; }
    int LastPower { get; }
    int FirstPenetrableCount { get; }
    int LastPenetrableCount { get; }
    float SpeedGrowthRate { get; }
    uint Attr { get; }
    string Name { get; }
    string Prefab { get; }
    Growth GrowthType { get; }
    float Impact { get; }
    int IconNo { get; }
    SkillAimingType Aiming { get; }
  }

  /// <summary>
  /// SkillMasterデータクラス
  /// </summary>
  public static class SkillMaster
  {
    /// <summary>
    /// このクラスを利用するまえに必ず一度だけ呼ぶこと
    /// </summary>
    public static void Init()
    {
      SkillRepository.Load();
    }

    /// <summary>
    /// SkillIdに該当するMasterデータを取得する
    /// </summary>
    public static ISkillEntity FindById(SkillId id)
    {
      return SkillRepository.entities.Find(entity => entity.Id == id);
    }
  }
}