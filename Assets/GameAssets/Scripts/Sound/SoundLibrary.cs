using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/SoundLibrary (Simple)")]
public class SoundLibrary : ScriptableObject
{
    [Serializable]
    public struct SfxEntry
    {
        public SfxType type;
        public AudioClip clip;
    }

    [Serializable]
    public struct BgmEntry
    {
        public BgmType type;
        public AudioClip clip;
    }

    [Header("BGM")]
    public BgmEntry[] bgm;

    [Header("SFX")]
    public SfxEntry[] sfx;

    public AudioClip GetBgm(BgmType type)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (bgm[i].type == type)
                return bgm[i].clip;
        }
        return null;
    }

    public AudioClip GetSfx(SfxType type)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (sfx[i].type == type)
                return sfx[i].clip;
        }
        return null;
    }
}