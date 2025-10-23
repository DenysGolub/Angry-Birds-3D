using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ChangeVolumeMixer: MonoBehaviour
{
    public AudioMixer VolumeMixer;
    public Slider MusicSlider;
    public Slider SfxSlider;

    public void SetVolumeMusic()
    {
        float sliderValue = MusicSlider.value;
        VolumeMixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }
    public void SetVolumeSFX()
    {
        float sliderValue = SfxSlider.value;
        VolumeMixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }
    

}