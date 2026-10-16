# Setup Hair for MA

[English](README.md) | 한국어 | [日本語](README_JA.md)

> 헤어를 MA Bone Proxy로 VRChat 아바타의 머리에 붙이고, 몸통·위팔 콜라이더를 PhysBone에 추가하는 도구입니다.

## 요구 사항

- Unity 2022.3.22f1
- [Modular Avatar](https://modular-avatar.nadena.dev/)

## 설치

[VPM 저장소](https://koz39.github.io/vpm-listing/)에서 "Add to VCC"로 저장소를 추가한 뒤, VCC 또는 ALCOM의 프로젝트 관리에서 Setup Hair for MA 패키지를 추가합니다.

## 사용법

1. 헤어 프리팹을 아바타 안에 넣습니다.
2. 하이어라키에서 헤어 프리팹을 우클릭하고 Modular Avatar > Setup Hair를 선택합니다.
3. 헤어 프리팹 안의 Head 또는 Hair 본에 MA Bone Proxy가 추가되어 헤어가 아바타의 머리를 따라갑니다.
   - Neck이나 Head 본을 줄인 아바타에서는 "스케일 맞춤"이 활성화됩니다.
   - 헤어 PhysBone에는 닿는 범위의 몸통·위팔 콜라이더가 추가됩니다.
4. 설정할 수 없는 헤어나 머리를 따라가지 않는 오브젝트가 있으면 창에 표시됩니다.

## 주의 사항

- Modular Avatar 공식 도구가 아닙니다. 이 도구에 관한 문의는 Modular Avatar 개발자에게 보내지 말아 주세요.
- Armature 바로 아래에서 Head 또는 Hair 본 외의 오브젝트는 머리를 따라가지 않습니다.
- 콜라이더 자동 추가는 아직 실험적인 기능입니다. PhysBone 길이를 기준으로 추가되므로 결과를 확인하고 필요하면 직접 조정해 주세요. 이미 콜라이더가 지정된 PhysBone에는 추가하지 않습니다.
- 아바타의 기존 콜라이더만 사용하며, 콜라이더를 생성하지는 않습니다. 몸통·위팔 콜라이더가 없는 아바타에서는 추가되지 않습니다.
- 콜라이더 자동 추가는 상단 메뉴의 Tools > Setup Hair for MA > Auto-Add PhysBone Colliders (Experimental)로 비활성화할 수 있습니다.
