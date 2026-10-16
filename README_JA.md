# Setup Hair for MA

[English](README.md) | [한국어](README_KO.md) | 日本語

> 髪型をMA Bone ProxyでVRChatアバターの頭に取り付け、胴体・上腕のコライダーをPhysBoneに追加するツールです。

## 動作環境

- Unity 2022.3.22f1
- [Modular Avatar](https://modular-avatar.nadena.dev/)

## インストール

[VPMリポジトリ](https://koz39.github.io/vpm-listing/)で「Add to VCC」を押してリポジトリを追加した後、VCCまたはALCOMのプロジェクトの管理画面でSetup Hair for MAパッケージを追加します。

## 使い方

1. 髪型のプレハブをアバターの中に配置します。
2. ヒエラルキーで髪型のプレハブを右クリックし、Modular Avatar > Setup Hair を選択します。
3. 髪型のプレハブ内にあるHeadまたはHairボーンにMA Bone Proxyが追加され、髪型がアバターの頭に追従します。
   - NeckまたはHeadボーンを縮小したアバターでは、「スケールを合わせる」が有効になります。
   - 髪型のPhysBoneには、届く範囲の胴体・上腕のコライダーが追加されます。
4. 設定できない髪型や頭に追従しないオブジェクトがある場合は、ウィンドウに表示されます。

## 注意事項

- Modular Avatarの右クリックメニューに項目を追加する非公式ツールです。
- Armature直下のオブジェクトのうち、頭に追従するのはHeadまたはHairボーンのみです。
- コライダーの自動追加は実験的な機能です。
  - PhysBoneの長さを基準にコライダーを選ぶため、結果を確認し、必要に応じて手動で調整してください。
  - すでにコライダーが設定されているPhysBoneはスキップします。
  - アバターの既存のコライダーのみを使用し、作成はしないため、胴体・上腕のコライダーがないアバターには追加されません。
  - 上部メニューの Tools > Setup Hair for MA > Auto-Add PhysBone Colliders (Experimental) で有効・無効を切り替えられます。
