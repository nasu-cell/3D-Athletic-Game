# 3D Athletic

さまざまなギミックが配置されたステージを、ストックとチェックポイントを頼りにゴールまで進む Unity 製の 3D アスレチックゲームです。

> ⚠️ 開発中（v0.1.0）です。仕様や内容は今後変更される可能性があります。

> *(代表画像をここに追加)*

---

## 目次

- [概要](#概要)
- [開発情報](#開発情報)
- [プレイ方法](#プレイ方法)
- [使用技術](#使用技術)
- [実装内容](#実装内容)
- [こだわり・工夫](#こだわり工夫)
- [ディレクトリ構成](#ディレクトリ構成)
- [スクリーンショット](#スクリーンショット)
- [使用アセット](#使用アセット)

---

## 概要

### どんなゲームか

| 項目 | 詳細 |
|------|------|
| ジャンル | 3D アスレチック（アクション） |
| プラットフォーム | PC（Windows） |

1. タイトル画面で「Game Start」を押してステージを開始します。
2. 移動床・落下ブロック・点滅ブロック・振り子・転がる丸太などのギミックを越えながらゴールを目指します。
3. 落下やトラップに触れるとストックが 1 つ減り、最後に通過したチェックポイントから再開します。
4. ストックが 0 になるとゲームオーバー。「ReStart」で最初から、「Title」でタイトルへ戻ります。
5. ゴールに触れるとクリア画面へ移動します。

### 制作の目的

> *(何を目指して制作したかをここに記入)*

---

## 開発情報

| 項目 | 詳細 |
|------|------|
| 開発期間 | 2026年6月〜（開発中） |
| 開発人数 | 1人（個人開発） |
| 担当 | 企画・プログラム・ステージ制作 |

---

## プレイ方法

### 公開リンク / ダウンロード

> *(公開リンク・ビルドのダウンロード先をここに追加)*

### プレイ動画

> *(YouTube のプレイ動画リンクをここに追加)*

### 動作環境

- Unity 6000.4.10f1 (Unity 6) 以降
- Git（UniTask を Git URL で参照しているため、初回起動時に Package Manager が自動で取得します）

### Unity エディタで起動する手順

```bash
# 1. リポジトリのクローン
git clone https://github.com/nasu-cell/3D-Athletic-Game.git

# 2. Unity Hub でプロジェクトを開く
#    [Add] → クローンしたフォルダを選択（Unity 6000.4.10f1）

# 3. 最初のシーンを開く
#    Assets/Scenes/TitleScene.unity

# 4. Play ボタンでゲーム開始
```

### 操作方法

| 操作 | キー |
|------|------|
| 移動 | `W` `A` `S` `D` |
| ダッシュ | `Shift`（押している間） |
| ジャンプ | `Space` |
| 視点移動 | マウス |
| メニュー操作 | マウスクリック |

移動方向はカメラの向きが基準です。

---

## 使用技術

| カテゴリ | 技術 |
|----------|------|
| **ゲームエンジン** | Unity 6000.4.10f1 (Unity 6) |
| **言語** | C# |
| **グラフィクス** | Universal Render Pipeline (URP) 17.4.0 |
| **カメラ** | Cinemachine 3.1.7 |
| **UI** | uGUI / TextMesh Pro |
| **入力** | Input System 1.19.0 |
| **非同期処理** | [UniTask](https://github.com/Cysharp/UniTask)（Git URL で導入） |
| **バージョン管理** | Git / GitHub |
| **開発環境** | Windows 11 / Visual Studio |

---

## 実装内容

### 全体構成

```
GameManager          ストック・リスポーン・ゲーム開始/オーバーの管理
  │
  ├─ UIManager       ストック表示・ゲームオーバー表示（UniTask で演出を待機）
  ├─ SpawnPoint[]    チェックポイント（通過で savedSpawnPointID を更新）
  └─ Player (生成)    PlayerController：移動・ダッシュ・ジャンプ
                     └ Cinemachine カメラの TrackingTarget に設定

StockDown            触れると GameManager.StockDecrease() を呼ぶ（落下エリア・振り子・丸太）
BlinkBlock           GameManager.isGamePlaying を見て点滅を一時停止
SceneMover           シーン遷移（GoalPoint・タイトル/クリア画面のボタンから呼び出し）
```

### 1. ストック・リスポーン

トラップ（`StockDown`）に触れるとプレイヤーを破棄し、`GameManager` がストックを減らします。ストックが残っていれば UI の演出を `await` で待ってから、記録したチェックポイントにプレイヤーを生成し直し、カメラの追従対象を差し替えます。

```csharp
public async UniTask StockDecrease()
{
    isGamePlaying = false;
    stock--;

    if (stock <= 0)
    {
        await GameOver();
    }
    else
    {
        await ReStart();
    }
}

private void SpawnPlayer()
{
    GameObject player = Instantiate(
        playerPrefab,
        spawnPoints[savedSpawnPointID].transform.position,
        Quaternion.identity
    );

    // 新しく生成したPlayerをカメラのTracking Targetに設定
    cinemachineCamera.Target.TrackingTarget = player.transform;
}
```

### 2. カメラ基準の移動

カメラの正面・右方向ベクトルから Y 成分を除いて正規化し、入力と合成して進行方向を求めます。キャラクターは進行方向へ `Slerp` で滑らかに回転します。

```csharp
Vector3 camForward = mainCameraTransform.forward;
Vector3 camRight = mainCameraTransform.right;
camForward.y = 0;
camRight.y = 0;
camForward.Normalize();
camRight.Normalize();
moveDirection = (camForward * moveInput.y) + (camRight * moveInput.x);
moveDirection.Normalize();
Quaternion targetRotetion = Quaternion.LookRotation(moveDirection);
transform.rotation = Quaternion.Slerp(transform.rotation, targetRotetion, Time.deltaTime * 15f);
```

### 3. ステージギミック

| ギミック | 仕組み |
|----------|--------|
| 移動床 (FloorMove) | 開始位置からの距離が `range` を超えたら向きを反転して往復。乗ったプレイヤーを子オブジェクトにして一緒に動かします。 |
| 落下ブロック (FallBlock) | 乗っている時間を計測し、`timeLimit` を超えると落下。一定距離落ちたら非表示にして元の位置へ戻し、`respawnTime` 後に復活します。 |
| 点滅ブロック (BlinkBlock) | 1 秒ごとの予告音を 3 回鳴らしてから出現/消滅を切り替えます（消滅時は半透明＋当たり判定なし）。 |
| ジャンプ台 (JumpFloor) | 接触したプレイヤーの Rigidbody に上向きの力を加えます。 |
| 振り子 (Pendulum) | `Mathf.Sin` で角度を計算し、配置時の向きを基準に揺らします。`timeOffset` で複数の振り子のタイミングをずらせます。 |
| 丸太 (Log / LogSpawner) | スポナーが一定間隔で丸太を生成し、丸太は `AddForce` で転がり、一定距離進むと自身を破棄します。 |
| チェックポイント (SpawnPoint) | 通過すると ID を `GameManager` に記録し、目印のシリンダーを非表示にします。 |
| ゴール (GoalPoint) | 触れるとクリア画面へ遷移します。 |

---

## こだわり・工夫

### UniTask による待機処理の一本化

- **課題:** ストック表示 → リスポーン → ゲーム再開のように「演出を待ってから次へ進む」処理が複数のクラスにまたがり、コルーチンでは呼び出し側で完了を待つのが煩雑でした。
- **解決:** `GameManager` / `UIManager` / `BlinkBlock` / `FallBlock` の待機処理を UniTask の `async/await` に置き換えました。
- **設計意図:** `await uiManager.StockChange(stock)` のように処理の順序をそのままコードで表現でき、戻り値で完了を待てるため流れが追いやすくなります。`FallBlock` では `destroyCancellationToken` を渡し、オブジェクト破棄時に待機が残らないようにしています。

### リスポーン中は点滅ブロックを止める

- **課題:** ミス後の演出中も点滅ブロックが切り替わり続けると、再開直後に足場が消えるなど理不尽な状況が起こり得ます。
- **解決:** `GameManager.isGamePlaying` をフラグにし、`BlinkBlock` は `UniTask.WaitUntil` でプレイ中になるまで待機、途中でミスした場合はその周期をスキップします。
- **設計意図:** ゲームの状態を `GameManager` に集約し、ギミック側はそれを参照するだけにすることで、ギミックを増やしても同じ仕組みで制御できます。

### 配置の向きに依存しない振り子

- **課題:** 振り子をステージ上でさまざまな向きに配置すると、単純に Z 軸回転を上書きするだけでは向きが崩れてしまいます。
- **解決:** 起動時の `localRotation` を `baseRotation` として記憶し、それに揺れの回転を掛け合わせています。
- **設計意図:** エディタ上で自由に回転させて配置できるようにし、ステージ制作の手間を減らすためです。

### 今後の課題

- [ ] ステージの追加
- [ ] 加速/減速エリア（SpeedUpArea / SpeedDownArea）の配置（実装済み・未配置）
- [ ] *(記入)*

---

## ディレクトリ構成

```
Assets/
├── Animator/       # プレイヤーのアニメーターコントローラー
├── Materials/      # ステージ用マテリアル
├── Prefabs/        # Player・各ステージギミックのプレハブ、UI 画像
├── Scenes/
│   ├── TitleScene.unity    # タイトル画面
│   ├── GameScene.unity     # メインのステージ
│   └── ClearScene.unity    # クリア画面
├── Scripts/
│   ├── Manager/
│   │   ├── GameManager.cs      # ストック・リスポーン・ゲーム開始の管理
│   │   ├── UIManager.cs        # ストック表示・ゲームオーバー表示
│   │   └── SceneMover.cs       # シーン遷移
│   ├── Player/
│   │   └── PlayerController.cs # 移動・ダッシュ・ジャンプ
│   └── StageGimmick/
│       ├── BlinkBlock.cs       # 点滅ブロック
│       ├── FallBlock.cs        # 落下ブロック
│       ├── FloorMove.cs        # 移動床
│       ├── JumpFloor.cs        # ジャンプ台
│       ├── Pendulum.cs         # 振り子
│       ├── Log.cs              # 転がる丸太
│       ├── LogSpawner.cs       # 丸太の生成
│       ├── StockDown.cs        # 触れるとストック減少
│       ├── SpawnPoint.cs       # チェックポイント
│       ├── GoalPoint.cs        # ゴール
│       └── SpeedChange.cs      # 加速/減速エリア（未配置）
├── Settings/       # URP の設定
└── InputSystem_Actions.inputactions   # 入力アクション定義（カメラ操作で使用）
```

外部アセットは [使用アセット](#使用アセット) を参照してください。

---

## スクリーンショット

> *(作品の魅力や実装機能が伝わる画像をここに追加)*

---

## 使用アセット

| 素材名 | 提供元 | 用途 | ライセンス |
|--------|--------|------|------------|
| [Human Animations](https://assetstore.unity.com/publishers/26529) | Kevin Iglesias | プレイヤーのアニメーション | Unity Asset Store EULA |
| RPG Monster DUO PBR Polyart | *(記入)* | *(記入)* | *(記入)* |
| npc_casual_set_00 | *(記入)* | *(記入)* | *(記入)* |
| Casual Game Sounds U6 | *(記入)* | 効果音 | *(記入)* |

本プロジェクトに含まれるサードパーティアセットは、各提供元のライセンスに従います。
