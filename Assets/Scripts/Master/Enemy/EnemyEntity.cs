using UnityEngine;

namespace MyGame.Master
{
  public class EnemyEntity : ScriptableObject, IEnemyEntity, IConvertibleCsvText
  {
    //=========================================================================
    // Inspector
    //=========================================================================
    public string _id;

    public int _no;

    public string _name;

    public int _hp;

    public int _power;

    public float _speed;

    public float _mass;

    public string _attackAttributes;

    public string _weakAttributes;

    public string _resistAttributes;

    public string _nullfiedAttributes;

    public string _skillId;

    public int _exp;

    //=========================================================================
    // Variables that need to be converted.
    // Inspectorで設定された文字列やデータの中で変換する必要があるパラメータを定義
    // 以下に定義されたデータはInitのタイミングで使える形式に変換される。
    //=========================================================================
    private EnemyId id;
    private SkillId skillId;
    private string prefabPath;
    private uint attackAttributes;
    private uint weakAttributes;
    private uint resistAttributes;
    private uint nullfiedAttributes;

    //=========================================================================
    // Property
    //=========================================================================
    public EnemyId Id => id;

    public int No => _no;

    public string Name => _name;

    public float HP => _hp;

    public float Power => _power;

    public float Speed => _speed;

    public float Mass => _mass;

    public uint AttackAttr => attackAttributes;

    public uint WeakAttr => weakAttributes;

    public uint ResistAttr => resistAttributes;

    public uint NullfiedAttr => nullfiedAttributes;

    public SkillId SkillId => skillId;

    public int Exp => _exp;

    public string PrefabPath => prefabPath;

    //=========================================================================
    // Method
    //=========================================================================
    public void Init()
    {
      if (!MyEnum.TryParse<EnemyId>(_id, out id)) {
        Logger.Error($"EnemyId {_id} parse failed.");
      }
      if (!MyEnum.TryParse<SkillId>(_skillId, out skillId)) {
        Logger.Error($"SkillId {_skillId} parse failed.");
      }

      {
        var no = _no.ToString("000");
        prefabPath = $"Enemy/{no}/{no}.prefab";
      }

      attackAttributes   = AttributeUtil.GetAttributesFromString(_attackAttributes);
      weakAttributes     = AttributeUtil.GetAttributesFromString(_weakAttributes);
      resistAttributes   = AttributeUtil.GetAttributesFromString(_resistAttributes);
      nullfiedAttributes = AttributeUtil.GetAttributesFromString(_nullfiedAttributes);
    }

    //=========================================================================
    // ToString
    //=========================================================================

    public static string CsvHeaderString()
    {
      return "Id,No,Name,HP,Power,Speed,Mass,AttackAttr,WeakAttr,ResistAttr,NullfiedAttr,SkillId,Exp";
    }

    public string ToCsvText()
    {
      string[] datas = { 
        _id,
        _no.ToString(),
        _name,
        _hp.ToString(),
        _power.ToString(),
        _speed.ToString(),
        _mass.ToString(),
        _attackAttributes,
        _weakAttributes,
        _resistAttributes,
        _nullfiedAttributes,
        _skillId.ToString(),
        _exp.ToString(),
      };

      return string.Join(",", datas);

    }
  }
}


