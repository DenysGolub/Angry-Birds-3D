using System;
using AngryBirds.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AngryBirds.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Music")] 
        [SerializeField] private AudioSource _gameMusic;
        [SerializeField] private AudioClip[] _levelMusic;
        [SerializeField] private AudioSource _levelEndMusic;
        [SerializeField] private AudioClip[] _levelEndClips;
        
        [Header("Birds Flying Sound Effects")]
        [SerializeField] private AudioSource _birdsFlyingSoundEffects;
        [SerializeField] private AudioClip[] _birdsSounds;
        
        [Header("Birds Special Ability Sound Effects")]
        [SerializeField] private AudioSource _birdsSpecialAbilityEffects;
        [SerializeField] private AudioClip[] _birdsSpecialAbilitySounds;
       
        [SerializeField] private AudioSource _birdDestroyed;
        
        [Header("Selected Birds Sound Effects")]
        [SerializeField] private AudioSource _selectedBirdsSoundEffects;
        [SerializeField] private AudioClip[] _selectedSounds;
        
        [Header("Slingshot Sound Effects")] 
        [SerializeField] private AudioSource _slingshotStretch;
        [SerializeField] private AudioSource _slingshotLaunch;

        [Header("Pigs Sound Effects")]
        [SerializeField] private AudioSource _pigsSoundEffects;
        
        [Header("Blocks Sound Effects")]
        [SerializeField] private AudioSource _blockDestroyed;
        [SerializeField] private AudioClip[] _blockDestroyedSounds;
        
        public static AudioManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _gameMusic.clip = _levelMusic[SceneManager.GetActiveScene().buildIndex - 1];
            _gameMusic.Play();
        }
        
        public void PlaySlingshotStretch()
        {
            _slingshotStretch.Play();
        }

        public void PlayLaunchSlingshot()
        {
            _slingshotLaunch.Play();
        }

        public void PlayPigDeath()
        {
            _pigsSoundEffects.Play();
        }

        public void PlaySelectedBirdsSoundEffects(BirdType birdType)
        {
            _selectedBirdsSoundEffects.clip = _selectedSounds[(int)birdType];
            _selectedBirdsSoundEffects.Play();
        }
        
        public void PlayBirdLaunch(BirdType birdType)
        {
            _birdsFlyingSoundEffects.clip = _birdsSounds[(int)birdType];
            _birdsFlyingSoundEffects.Play();
        }
        
        public void PlaySpecialAbility(BirdType birdType)
        {
            _birdsSpecialAbilityEffects.clip = _birdsSpecialAbilitySounds[(int)birdType];
            _birdsSpecialAbilityEffects.Play();
        }

        public void PlayBirdDeath()
        {
            _birdDestroyed.Play();
        }

        public void PlayDestroyedBlock(BlockType blockType)
        {
            _blockDestroyed.clip = _blockDestroyedSounds[(int)blockType];
            _blockDestroyed.Play();
        }

        public void PlayEndLevel(bool isWin)
        {
            _levelEndMusic.clip = _levelEndClips[Convert.ToInt32(isWin)];
            _levelEndMusic.Play();
        }
    }
}
