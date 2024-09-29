namespace MyGame.Master
{
  /// <summary>
  /// ReadOnlyのEnemyEntityインターフェース
  /// </summary>
  public interface IEnemyEntity
  {
    EnemyId Id { get; }
    int No { get; }
    string Name { get; }
    float HP { get; }
    float Power { get; }
    float Speed { get; }
    float Mass { get; }
    uint AttackAttr { get; }
    uint WeakAttr { get; }
    uint ResistAttr { get; }
    uint NullfiedAttr { get; }
    SkillId SkillId { get; }
    int Exp { get; }
    string PrefabPath { get; }
  }

  /// <summary>
  /// EnemyMasterデータクラス
  /// </summary>
  public static class EnemyMaster
  {
    /// <summary>
    /// このクラスを利用するまえに必ず一度だけ呼ぶこと
    /// </summary>
    public static void Init()
    {
      EnemyRepository.Load();
    }

    /// <summary>
    /// EnemyIdに該当するMasterデータを取得する
    /// </summary>
    public static IEnemyEntity FindById(EnemyId id)
    {
      return EnemyRepository.entities.Find(entity => entity.Id == id);
    }
  }
}