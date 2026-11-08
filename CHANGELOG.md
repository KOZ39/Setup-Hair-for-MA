# 변경 내역

## v0.5.0 (2026-11-08)

### Changed

- Setup Mesh Settings의 Bounds
  - Body 메시의 Extents 중 1 미만인 값이 있을 시 기본값 유지

## v0.4.0 (2026-11-07)

### Changed

- 헤어의 Armature 바로 아래에서 MA Bone Proxy를 추가하는 오브젝트
  - Head 또는 Hair 본이 없을 시 PhysBone 콜라이더를 제외한 유일한 오브젝트
  - 가장 가까운 아바타 본을 따라가는 PhysBone 콜라이더

### Fixed

- MA Bone Proxy로 이미 머리를 따라가는 헤어를 Head 또는 Hair 본이 없다며 건너뛰는 문제

## v0.3.0 (2026-11-02)

### Added

- Modular Avatar 우클릭 메뉴
  - Setup Mesh Settings

## v0.2.0 (2026-10-22)

### Changed

- 의상 프리팹 선택 시 Modular Avatar의 Setup Outfit 적용
- Modular Avatar 우클릭 메뉴 이름
  - Setup Hair → Setup Hair or Outfit

## v0.1.0 (2026-10-16)

### Added

- 최초 출시
