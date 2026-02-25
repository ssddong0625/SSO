using GameAssets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudionPanel : MonoBehaviour
{
    [Header("Sliders (0~1)")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;


    private void Awake()
    {
    }

    public void OpenAudioPanel()
    {
        gameObject.SetActive(true);
    }
    private void Start()
    {
        // 1) 슬라이더 초기값 세팅
        InitSlider(masterSlider, GameManager.instance.SoundManager.GetMaster());
        InitSlider(bgmSlider, GameManager.instance.SoundManager.GetBgmVol());
        InitSlider(sfxSlider, GameManager.instance.SoundManager.GetSfxVol());
        InitSlider(uiSlider, GameManager.instance.SoundManager.GetUiVol());

        // 2) 값 변경 이벤트 연결
        masterSlider.onValueChanged.AddListener(GameManager.instance.SoundManager.SetMaster);
        bgmSlider.onValueChanged.AddListener(GameManager.instance.SoundManager.SetBgmVol);
        sfxSlider.onValueChanged.AddListener(GameManager.instance.SoundManager.SetSfxVol);
        uiSlider.onValueChanged.AddListener(GameManager.instance.SoundManager.SetUiVol);
    }

    private void InitSlider(Slider s, float value)
    {
        if (s == null) return;
        s.minValue = 0f;
        s.maxValue = 1f;
        s.value = value;
    }

 
    public void TestUiClick()
    {
        GameManager.instance.SoundManager.PlayUi(SfxType.UI_Click);
    }
}
