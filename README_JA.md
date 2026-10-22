# Setup Hair for MA

[English](README.md) | [한국어](README_KO.md) | 日本語

> VRChatアバターの髪型と衣装を設定する、非公式のModular Avatarメニューです。

## 動作環境

- Unity 2022.3.22f1
- [Modular Avatar](https://modular-avatar.nadena.dev/)

## インストール

[VPMリポジトリ](https://koz39.github.io/vpm-listing/)で「Add to VCC」を押してリポジトリを追加した後、VCCまたはALCOMのプロジェクトの管理画面でSetup Hair for MAパッケージを追加します。

## 使い方

1. 髪型または衣装のプレハブをアバターの中に配置します。
2. ヒエラルキーでプレハブを右クリックし、Modular Avatar > Setup Hair or Outfit を選択します。
3. 髪型のプレハブ内にあるHeadまたはHairボーンにMA Bone Proxyが追加され、髪型がアバターの頭に追従します。
   - NeckまたはHeadボーンを縮小したアバターでは、「スケールを合わせる」が有効になります。
   - 髪型のPhysBoneには、届く範囲の胴体・上腕のコライダーが追加されます。
4. 衣装のプレハブは、Modular AvatarのSetup Outfitで設定されます。
5. 設定できない髪型や頭に追従しないオブジェクトがある場合は、ウィンドウに表示されます。衣装のエラーはSetup Outfitのウィンドウに表示されます。

## 注意事項

- Modular Avatarの右クリックメニューに項目を追加する非公式ツールです。
- Humanoid Animatorがあるか、Armature直下にHipsボーンがあるオブジェクトは衣装として設定されます。
- Armature直下のオブジェクトのうち、頭に追従するのはHeadまたはHairボーンのみです。
- コライダーの自動追加は実験的な機能です。
  - PhysBoneの長さを基準にコライダーを選ぶため、結果を確認し、必要に応じて手動で調整してください。
  - すでにコライダーが設定されているPhysBoneはスキップします。
  - アバターの既存のコライダーのみを使用し、作成はしないため、胴体・上腕のコライダーがないアバターには追加されません。
  - 上部メニューの Tools > Setup Hair for MA > Auto-Add PhysBone Colliders (Experimental) で有効・無効を切り替えられます。
