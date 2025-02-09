﻿using MyGame.Core.System;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MyGame.System
{
  /// <summary>
  /// リソースシステム
  /// 責務:リソースのロード及びキャッシュ、リソースの破棄
  /// </summary>
  public static class ResourceSystem
  {
    //=========================================================================
    // Inner class
    //=========================================================================

    /// <summary>
    /// キャッシュリソース、参照カウンタとリソースの参照を保持するのみ
    /// </summary>
    private class CachedResource
    {
      /// <summary>
      /// 参照カウンタ
      /// </summary>
      public uint Count = 0;

      /// <summary>
      /// リソース
      /// </summary>
      public UnityEngine.Object Resource = null;

      /// <summary>
      /// Spritesリソース
      /// </summary>
      public IList<Sprite> Sprites = null;

      /// <summary>
      /// コンストラクタで参照カウンタを1に設定
      /// </summary>
      /// <param name="resource"></param>
      public CachedResource(UnityEngine.Object resource)
      {
        this.Count = 1;
        this.Resource = resource;
      }

      /// <summary>
      /// コンストラクタで参照カウンタを1に設定
      /// </summary>
      /// <param name="resource"></param>
      public CachedResource(IList<Sprite> resource)
      {
        this.Count = 1;
        this.Sprites = resource;
      }
    }

    //=========================================================================
    // Variables
    //=========================================================================
    /// <summary>
    /// キャッシュ済リソース、keyはリソースのアドレス
    /// </summary>
    private static Dictionary<string, CachedResource> cache = new ();

    /// <summary>
    /// ロードカウンター
    /// </summary>
    private static int loadCounter = 0;

    //=========================================================================
    // Properties
    //=========================================================================
    /// <summary>
    /// ロード中ならばtrue
    /// </summary>
    public static bool IsLoading {
      get { return 0 < loadCounter; }
    }

    //=========================================================================
    // Methods
    //=========================================================================
    /// <summary>
    /// リソースの非同期ロードを行う
    /// </summary>
    /// <typeparam name="T">ロードするリソースの種類</typeparam>
    /// <param name="address">Addressable Asssetsで登録したAddress</param>
    /// <param name="post">ロード完了したリソースを受け取る関数</param>
    public static void Load<T>(string address, Action<T> post = null) where T : UnityEngine.Object
    {
      loadCounter++;

      Addressables.LoadAssetAsync<T>(address).Completed += op => {
        // ロード完了時コールバックを実行
        post?.Invoke(op.Result);

        loadCounter--;
        if (op.Result == null) {
          Logger.Error($"[ResourceManager.Load]:{address}がロードできませんでした。");
          return;
        }

        // 未キャッシュであればキャッシュ、キャッシュ済であれば参照カウンタを更新
        if (!cache.ContainsKey(address)) {
          cache[address] = new CachedResource(op.Result);
        }
        else {
          cache[address].Count++;
        }
      };
    }

    /// <summary>
    /// キャッシュ済のリソースを取得、参照カウンタは変化しない
    /// </summary>
    public static T GetCache<T>(string address) where T : UnityEngine.Object
    {
      if (!cache.ContainsKey(address)) {
        Logger.Error($"[ResourceManager.GetCache]: {address} is not found.");
        return null;
      }
      else {
        return cache[address].Resource as T;
      }
    }

    /// <summary>
    /// Multiple設定のSpriteをロードする
    /// </summary>
    public static void LoadSprites(string address, Action<IList<Sprite>> post = null)
    {
      loadCounter++;

      Addressables.LoadAssetAsync<IList<Sprite>>(address).Completed += op => {
        // ロード完了時コールバックを実行
        post?.Invoke(op.Result);

        loadCounter--;
        if (op.Result == null) {
          Logger.Error($"[ResourceManager.LoadSprites]:{address}がロードできませんでした。");
          return;
        }

        // 未キャッシュであればキャッシュ、キャッシュ済であれば参照カウンタを更新
        if (!cache.ContainsKey(address)) {
          cache[address] = new CachedResource(op.Result);
        }
        else {
          cache[address].Count++;
        }
      };
    }

    /// <summary>
    /// キャッシュ済のSpritesリソースを取得、参照カウンタは変化しない
    /// </summary>
    public static IList<Sprite> GetSpritesCache(string address)
    {
      if (!cache.ContainsKey(address)) {
        Logger.Log($"[ResourceManager.GetSpritesCache]: {address} is not found.");
        return null;
      }
      else {
        return cache[address].Sprites;
      }
    }

    /// <summary>
    /// リソースの破棄、参照カウンタが0になるまでは実際の破棄は行われない
    /// </summary>
    /// <param name="address">破棄するリソースのアドレス</param>
    public static void Unload(string address)
    {
      // リソースが存在しなければ何もしない
      if (!cache.ContainsKey(address)) {
        Logger.Warn($"ResourceManager.Unload:{address}は読み込まれていないか、すでに破棄されています。");
        return;
      }

      // キャッシュリソースを取得し、参照カウントを下げる
      var target = cache[address];
      target.Count--;

      // 参照カウントが0であればリソースを解放する
      if (target.Count == 0) {
        Addressables.Release(target.Resource);
        cache.Remove(address);
      }
    }

#if _DEBUG
    //=========================================================================
    // Debug
    //=========================================================================

    /// <summary>
    /// Debugger
    /// </summary>
    public static ResourceSystemDebugger Debugger { get; private set; } = new();

    /// <summary>
    /// ResourceSystemのDebugger
    /// </summary>
    public class ResourceSystemDebugger : IDebugable
    {
      private string __InputText = "";

      public string GetName()
      {
        return "ResourceSystem";
      }

      public void OnDebug()
      {
        GUILayout.Label($"Loading Counter = {loadCounter}");

        using (new GUILayout.VerticalScope(GUI.skin.box)) {
          __InputText = GUILayout.TextField(__InputText);

          using (new GUILayout.VerticalScope(GUI.skin.box)) {
            Util.ForEach(cache, (key, value) => {
              // 検索文字列が指定されている場合、該当しないリソースは表示しない
              if (__InputText != "" && !key.Contains(__InputText)) {
                return;
              }

              GUILayout.Label($"{key}:{value.Count}");
            });
          }
        }
      }
    }
#endif
  }
}