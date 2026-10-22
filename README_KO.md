# Setup Hair for MA

[English](README.md) | 한국어 | [日本語](README_JA.md)

> Modular Avatar 메뉴 하나로 VRChat 아바타의 헤어와 의상을 설정하는 도구입니다.

## 요구 사항

- Unity 2022.3.22f1
- [Modular Avatar](https://modular-avatar.nadena.dev/)

## 설치

[VPM 저장소](https://koz39.github.io/vpm-listing/)에서 "Add to VCC"로 저장소를 추가한 뒤, VCC 또는 ALCOM의 프로젝트 관리에서 Setup Hair for MA 패키지를 추가합니다.

## 사용법

1. 헤어나 의상 프리팹을 아바타 안에 넣습니다.
2. 하이어라키에서 프리팹을 우클릭하고 Modular Avatar > Setup Hair or Outfit을 선택합니다.
3. 헤어 프리팹 안의 Head 또는 Hair 본에 MA Bone Proxy가 추가되어 헤어가 아바타의 머리를 따라갑니다.
   - Neck이나 Head 본을 줄인 아바타에서는 "스케일 맞춤"이 활성화됩니다.
   - 헤어 PhysBone에는 닿는 범위의 몸통·위팔 콜라이더가 추가됩니다.
4. 의상 프리팹은 Modular Avatar의 Setup Outfit으로 설정됩니다.
5. 설정할 수 없는 헤어나 머리를 따라가지 않는 오브젝트가 있으면 창에 표시됩니다. 의상 오류는 Setup Outfit 창에 표시됩니다.

## 주의 사항

- Modular Avatar 우클릭 메뉴에 항목을 추가하는 비공식 도구입니다.
- Humanoid Animator가 있거나 Armature 바로 아래에 Hips 본이 있는 오브젝트는 의상으로 설정됩니다.
- Armature 바로 아래의 오브젝트 중 Head 또는 Hair 본만 머리를 따라갑니다.
- 콜라이더 자동 추가는 실험적인 기능입니다.
  - PhysBone 길이를 기준으로 콜라이더를 고르므로, 결과를 확인하고 필요하면 직접 조정해 주세요.
  - 콜라이더가 이미 지정된 PhysBone은 건너뜁니다.
  - 아바타의 기존 콜라이더만 사용하고 생성하지는 않으므로, 몸통·위팔 콜라이더가 없는 아바타에는 추가되지 않습니다.
  - 상단 메뉴의 Tools > Setup Hair for MA > Auto-Add PhysBone Colliders (Experimental)에서 활성화·비활성화할 수 있습니다.
