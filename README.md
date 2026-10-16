# Setup Hair for MA

English | [한국어](README_KO.md) | [日本語](README_JA.md)

> A tool that attaches hair to a VRChat avatar's head with MA Bone Proxy and adds torso and upper-arm colliders to its PhysBones.

## Requirements

- Unity 2022.3.22f1
- [Modular Avatar](https://modular-avatar.nadena.dev/)

## Installation

Click "Add to VCC" on the [VPM listing](https://koz39.github.io/vpm-listing/) to add the repository, then open your project's Manage page in VCC or ALCOM and add the Setup Hair for MA package.

## How to Use

1. Place the hair prefab inside your avatar.
2. Right-click the hair prefab in the Hierarchy and select Modular Avatar > Setup Hair.
3. MA Bone Proxy is added to the Head or Hair bone inside the hair prefab, and the hair follows the avatar's head.
   - If the avatar's Neck or Head bone is scaled down, "Match scale" is enabled.
   - The hair's PhysBones get the avatar's torso and upper-arm colliders within their reach.
4. A window lists any hair that can't be set up and any objects that won't follow the head.

## Notes

- This is not an official Modular Avatar tool. Please don't send questions about it to the Modular Avatar developers.
- For outfits that include hair, use Setup Outfit instead.
- Objects directly under the Armature other than the Head or Hair bone don't follow the head.
- Auto-adding colliders is still experimental. Colliders are picked by PhysBone length, so check the result and adjust them yourself if needed. Colliders aren't added to PhysBones that already have them.
- Only the avatar's existing colliders are used; none are created. Nothing is added for avatars without torso or upper-arm colliders.
- You can disable auto-adding colliders with Tools > Setup Hair for MA > Auto-Add PhysBone Colliders (Experimental) in the top menu.
