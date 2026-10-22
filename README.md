# Setup Hair for MA

English | [한국어](README_KO.md) | [日本語](README_JA.md)

> A tool that sets up hair and outfits on VRChat avatars from a single Modular Avatar menu.

## Requirements

- Unity 2022.3.22f1
- [Modular Avatar](https://modular-avatar.nadena.dev/)

## Installation

Click "Add to VCC" on the [VPM listing](https://koz39.github.io/vpm-listing/) to add the repository, then open your project's Manage page in VCC or ALCOM and add the Setup Hair for MA package.

## How to Use

1. Place the hair or outfit prefab inside your avatar.
2. Right-click the prefab in the Hierarchy and select Modular Avatar > Setup Hair or Outfit.
3. MA Bone Proxy is added to the Head or Hair bone inside the hair prefab, and the hair follows the avatar's head.
   - On avatars with a scaled-down Neck or Head bone, "Match scale" is enabled.
   - The hair's PhysBones get the torso and upper-arm colliders within their reach.
4. Outfit prefabs are set up with Modular Avatar's Setup Outfit.
5. A window lists any hair that can't be set up and any objects that won't follow the head. Outfit errors appear in the Setup Outfit window.

## Notes

- This is an unofficial tool that adds an item to the Modular Avatar right-click menu.
- Objects with a Humanoid Animator or a Hips bone directly under the Armature are set up as outfits.
- Of the objects directly under the Armature, only the Head or Hair bone follows the head.
- Auto-adding colliders is experimental.
  - Colliders are picked by PhysBone length, so check the result and adjust them yourself if needed.
  - PhysBones that already have colliders are skipped.
  - Only the avatar's existing colliders are used and none are created, so nothing is added for avatars without torso or upper-arm colliders.
  - You can enable or disable it with Tools > Setup Hair for MA > Auto-Add PhysBone Colliders (Experimental) in the top menu.
