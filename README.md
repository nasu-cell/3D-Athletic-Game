# 3D Athletic

Unity 製の 3D アスレチックゲームです。
さまざまなギミックが配置されたステージを進み、ゴールを目指します。
落下やトラップに触れるとストックが 1 つ減り、直前に通過したチェックポイントから再開します。ストックが 0 になるとゲームオーバーです。

> ⚠️ 開発中（v0.1.0）です。仕様や内容は今後変更される可能性があります。

## 動作環境

| 項目 | 内容 |
| --- | --- |
| Unity | 6000.4.10f1 (Unity 6) |
| レンダリング | Universal Render Pipeline (URP) 17.4.0 |
| 入力 | Input System 1.19.0 |
| カメラ | Cinemachine 3.1.7 |
| UI | uGUI / TextMeshPro |

## 遊び方

### 操作方法

| 操作 | キー |
| --- | --- |
| 移動 | `W` `A` `S` `D` |
| ダッシュ | `Shift`（押している間） |
| ジャンプ | `Space` |
| 視点移動 | マウス |

移動方向はカメラの向きが基準です。

### ルール

- 最大ストック数はインスペクターで設定します。
- 落下エリアなどの危険な場所に触れるとストックが 1 つ減ります。
- チェックポイント（SpawnPoint）を通過すると、次回のリスタート地点として記録されます。
- ストックが残っていれば、記録したチェックポイントから再開します。
- ストックが 0 になるとゲームオーバー画面が表示されます。

## ステージギミック

| ギミック | 説明 |
| --- | --- |
| 移動床 (FloorMove) | 指定した方向に往復する床。乗るとプレイヤーも一緒に動きます。 |
| 落下ブロック (FallBlock) | 一定時間乗っていると落下し、しばらくすると元の位置に復活します。 |
| 点滅ブロック (BlinkBlock) | 一定間隔で出現と消滅を繰り返します。切り替わる前に音で知らせます。赤と青の 2 種類があります。 |
| ジャンプ台 (JumpFloor) | 踏むと大きく跳ね上がります。 |
| 振り子 (Pendulum) | 一定のリズムで左右に振れる障害物です。 |
| 丸太 (Log / LogSpawner) | スポナーから一定間隔で丸太が転がってきます。 |
| 加速・減速エリア (SpeedChange) | 範囲内にいる間、移動速度が変化します。 |
| ストックダウン (StockDown) | 触れるとストックが減ります。 |
| チェックポイント (SpawnPoint) | 通過するとリスタート地点が更新されます。 |

## 起動方法

1. このリポジトリをクローンします。
   ```bash
   git clone https://github.com/nasu-cell/3D-Athletic-Game.git
   ```
2. Unity Hub で Unity **6000.4.10f1** をインストールします。
3. Unity Hub の「追加」からプロジェクトフォルダーを開きます。
4. `Assets/Scenes/GameScene.unity` を開き、再生ボタンを押します。

## プロジェクト構成

```
Assets/
├── Scenes/         GameScene（メインのステージ）
├── Prefabs/        Player・各ステージギミックのプレハブ、UI 画像
├── Scripts/
│   ├── Manager/        GameManager（ストック・リスポーン管理）
│   │                   UIManager（ストック表示・ゲームオーバー表示）
│   ├── Player/         PlayerController（移動・ダッシュ・ジャンプ）
│   └── StageGimmick/   各ステージギミックのスクリプト
├── Animator/       プレイヤーのアニメーターコントローラー
└── Materials/      ステージ用マテリアル
```

## 使用アセット

- [Kevin Iglesias – Human Animations](https://assetstore.unity.com/publishers/26529)
- RPG Monster DUO PBR Polyart
- npc_casual_set_00
- Casual Game Sounds U6

各アセットのライセンスは、配布元の規約に従います。

## 今後の予定

- ゲームオーバー後のリスタート処理
- ゴール判定とクリア画面
- ステージの追加
