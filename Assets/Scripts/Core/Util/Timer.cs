using UnityEngine;

/// <summary>
/// 時間経過を管理するクラス
/// </summary>
public class Timer
{
  /// <summary>
  /// 制限時間
  /// </summary>
  private float timeLimit = 0f;

  /// <summary>
  /// 経過時間
  /// </summary>
  private float timer = 0f;

  /// <summary>
  /// タイマー稼働中
  /// </summary>
  public bool IsRunning {
    get { return 0 < timeLimit; }
  }

  /// <summary>
  /// 経過割合
  /// </summary>
  public float Rate 
  {
    get 
    {
      // 0除算対策
      if (timeLimit <= 0) {
        return 0;
      }

      return Mathf.Clamp(timer / timeLimit, 0, 1f);
    }
  }

  /// <summary>
  /// 開始
  /// </summary>
  public void Start(float time)
  {
    this.timeLimit = time;
    timer = 0f;
  }

  /// <summary>
  /// 更新
  /// </summary>
  public void Update(float deltaTime)
  {
    if (!IsRunning) {
      return;
    }

    if (timeLimit < timer) {
      Stop();
      return;
    }

    timer += deltaTime;
  }

  /// <summary>
  /// 停止
  /// </summary>
  public void Stop()
  {
    timeLimit = 0;
    timer = 0;
  }
}
