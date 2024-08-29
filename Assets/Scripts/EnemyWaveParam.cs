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
  /// Wave����������G�̍��v��
  /// </summary>
  public int TotalEnemyCount {
    get { return WaveCount * EnemyAmountPerWave; }
  }

  /// <summary>
  /// �N�_�ƂȂ�p�x�A���̃p�����[�^��Shape.Circle�̂Ƃ��̂ݗL��
  /// </summary>
  public float OriginAngle { get; set; }

  /// <summary>
  /// Wave���ɓK�p�����p�x�̃I�t�Z�b�g
  /// </summary>
  public float WaveOffsetAngle { get; set; }

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
}
