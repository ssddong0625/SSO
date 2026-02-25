using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Library")]
    [SerializeField] private SoundLibrary library;

    [Header("Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource; // 없으면 sfxSource로 대체

    [Header("Volume")]
    [Range(0, 1)][SerializeField] private float master = 1f;
    [Range(0, 1)][SerializeField] private float bgmVol = 1f;
    [Range(0, 1)][SerializeField] private float sfxVol = 1f;
    [Range(0, 1)][SerializeField] private float uiVol = 1f;

    public float GetMaster() => master;
    public float GetBgmVol() => bgmVol;
    public float GetSfxVol() => sfxVol;
    public float GetUiVol() => uiVol;

    // ---------- BGM ----------
    public void PlayBgm(BgmType type, bool loop = true)
    {
        if (library == null || bgmSource == null) return;

        var clip = library.GetBgm(type);
        if (clip == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.volume = master * bgmVol;
        bgmSource.Play();
    }

    public void StopBgm()
    {
        if (bgmSource == null) return;
        bgmSource.Stop();
    }

    // ---------- SFX ----------
    public void PlaySfx(SfxType type, float volume = 1f)
    {
        if (library == null || sfxSource == null) return;

        var clip = library.GetSfx(type);
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, master * sfxVol * volume);
    }

    // UI도 SfxType에서 같이 관리하는 방식
    public void PlayUi(SfxType type, float volume = 1f)
    {
        if (library == null) return;

        var clip = library.GetSfx(type);
        if (clip == null) return;

        var src = (uiSource != null) ? uiSource : sfxSource;
        if (src == null) return;

        src.PlayOneShot(clip, master * uiVol * volume);
    }

    // ---------- Volume ----------
    public void SetMaster(float v) => master = Mathf.Clamp01(v);
    public void SetBgmVol(float v)
    {
        bgmVol = Mathf.Clamp01(v);
        if (bgmSource != null) bgmSource.volume = master * bgmVol; // 재생 중 즉시 반영
    }
    public void SetSfxVol(float v) => sfxVol = Mathf.Clamp01(v);
    public void SetUiVol(float v) => uiVol = Mathf.Clamp01(v);
}