using UnityEngine;

/// <summary>
/// �GWave�p�����[�^�AWave�̌`���o������G�̐���ʒu�Ȃǂ̏���ێ�����
/// </summary>
public class EnemyWaveParam
{
  /// <summary>
  /// �o������GID
  /// </summary>
  public EnemyId Id { get; set; }

  /// <summary>
  /// Wave�̌`��
  /// </summary>
  public WaveShape Shape { get; set; }

  /// <summary>
  /// Wave�̋N�_
  /// </summary>
  public Vector3 BasePosition { get; set; }

  /// <summary>
  /// Wave���G�𐶐�����G���A
  /// </summary>
  public Vector3 Area { get; set; }

  /// <summary>
  /// Wave��
  /// </summary>
  public int WaveCount { get; set; }

  /// <summary>
  /// ����Wave����������܂ł̊Ԋu
  /// </summary>
  public float WaveInterval { get; set; }

  /// <summary>
  /// 1��Wave����������G�̐�
  /// </summary>
  public int EnemyAmountPerWave { get; set; }

  /// <summary>
  /// ����Wave����������܂ł̑ҋ@����
  /// </summary>
  public float WaitTime { get; set; } = 0;

  /// <summary>
  /// �N�_�ƂȂ�p�x�A���̃p�����[�^��Shape.Circle�̂Ƃ��̂ݗL��
  /// </summary>
  public float OriginAngle { get; set; }

  /// <summary>
  /// Wave���ɓK�p�����p�x�̃I�t�Z�b�g
  /// </summary>
  public float WaveOffsetAngle { get; set; }

  /// <summary>
  /// Wave���ɓK�p�����I�t�Z�b�g�Az�͊p�x�Ɏg��
  /// </summary>
  private Vector3 waveOffset;

  /// <summary>
  /// �o���ʒu��X�����𔽓]����
  /// ���̃p�����[�^�[��Sphae.Line�̂Ƃ��̂ݗL��
  /// </summary>
  public bool InverseX { get; set; }

  /// <summary>
  /// �o���ʒu��Y�����𔽓]����
  /// ���̃p�����[�^�[��Sphae.Line�̂Ƃ��̂ݗL��
  /// </summary>
  public bool InverseY { get; set; }

  /// <summary>
  /// �o���ʒu��Z�����𔽓]����
  /// ���̃p�����[�^�[��Sphae.Line�̂Ƃ��̂ݗL��
  /// </summary>
  public bool InverseZ { get; set; }

  /// <summary>
  /// Wave����������G�̍��v��
  /// </summary>
  public int TotalEnemyCount {
    get { return WaveCount * EnemyAmountPerWave; }
  }

  /// <summary>
  /// Wave���ɓK�p�����X�����I�t�Z�b�g
  /// </summary>
  public float WaveOffsetX {
    get { return waveOffset.x; }
    set { waveOffset.x = value; }
  }

  /// <summary>
  /// Wave���ɓK�p�����Y�����I�t�Z�b�g
  /// </summary>
  public float WaveOffsetY {
    get { return waveOffset.y; }
    set { waveOffset.y = value; }
  }

  /// <summary>
  /// Wave���ɓK�p�����Z�����I�t�Z�b�g
  /// </summary>
  public float WaveOffsetZ {
    get { return waveOffset.z; }
    set { waveOffset.z = value; }
  }

  /// <summary>
  /// �C���X�^���X�𕡐�����
  /// </summary>
  public EnemyWaveParam Clone()
  {
    var param = new EnemyWaveParam();

    param.Id = this.Id;
    param.Shape = this.Shape;
    param.BasePosition = this.BasePosition;
    param.Area = this.Area;
    param.WaveCount = this.WaveCount;
    param.WaveInterval = this.WaveInterval;
    param.EnemyAmountPerWave = this.EnemyAmountPerWave;
    param.WaitTime = this.WaitTime;
    param.OriginAngle = this.OriginAngle;
    param.WaveOffsetAngle = this.WaveOffsetAngle;
    param.waveOffset = this.waveOffset;
    param.InverseX = this.InverseX;
    param.InverseY = this.InverseY;
    param.InverseZ = this.InverseZ;

    return param;
  }

  static public EnemyWaveParam Make(EnemyWaveSettings setting)
  {
    var param = new EnemyWaveParam();

    if (MyEnum.TryParse<EnemyId>(setting.EnemyId, out var enemyId)) {
      param.Id = enemyId;
    } else {
      Logger.Error($"[EnemyWaveParam] EnemyId = {setting.EnemyId} is not defined.");
    }

    if (MyEnum.TryParse<WaveShape>(setting.WaveShape, out var shape)) {
      param.Shape = shape;
    } else {
      Logger.Error($"[EnemyWaveParam] Shape = {setting.WaveShape} is not defined.");
    }

    param.BasePosition       = setting.transform.position;
    param.Area               = setting.transform.localScale;
    param.WaveCount          = setting.WaveCount;
    param.WaveInterval       = setting.WaveInterval;
    param.EnemyAmountPerWave = setting.EnemyAmountPerWave;
    param.WaitTime           = setting.WaitTime;
    param.OriginAngle        = setting.OriginAngle;
    param.WaveOffsetAngle    = setting.WaveOffsetAngle;
    param.waveOffset         = setting.WaveOffset;
    param.InverseX           = setting.InverseX;
    param.InverseY           = setting.InverseY;
    param.InverseZ           = setting.InverseZ;

    return param;
  }
}
