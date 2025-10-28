using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AngryBirds.UI
{
    public class ChangeVolumeMixer: MonoBehaviour
    {
        [SerializeField] 
        private AudioMixer _volumeMixer;
    
        [SerializeField] 
        private Slider _musicSlider;
    
        [SerializeField] 
        private Slider _sfxSlider;

        public void SetVolumeMusic()
        {
            float sliderValue = _musicSlider.value;
            _volumeMixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
        }
        public void SetVolumeSFX()
        {
            float sliderValue = _sfxSlider.value;
            _volumeMixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
        }
    

    }
}