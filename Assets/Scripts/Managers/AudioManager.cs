using System;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    [Header("Music")] 
    public AudioSource TitleMusic;

    public AudioSource GameMusic;
    public AudioClip[] LevelMusic;
    public AudioSource LevelEndMusic;
    public AudioClip[] LevelEndClips;
    
    [Header("Birds Sound Effects")]
    public AudioSource BirdsSoundEffects;
    public AudioClip[] BirdsSounds;
    
    public AudioSource BirdsSpecialAbilityEffects;
    public AudioClip[] BirdsSpecialAbilitySounds;

    public AudioSource BirdDestroyed;
    
    [Header("Selected Birds Sound Effects")]
    public AudioSource SelectedBirdsSoundEffects;
    public AudioClip[] SelectedSounds;
    
    [Header("Slingshot Sound Effects")] 
    public AudioSource SlingshotStrech;

    public AudioSource SlingshotLaunch;
    [Header("Pigs Sound Effects")]
    public AudioSource PigsSoundEffects;
    
    [Header("Blocks Sound Effects")]
    public AudioSource BlockDestroyed;
    public AudioClip[] BlockDestroyedSounds;
    public static AudioManager Instance;

    void Awake()
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

    void Start()
    {
        GameMusic.clip = LevelMusic[(int)SceneManager.GetActiveScene().buildIndex-1];
        GameMusic.Play();
    }
    
    public void PlaySlingshotStretch()
    {
        SlingshotStrech.Play();
    }

    public void PlayLaunchSlingshot()
    {
        SlingshotLaunch.Play();
    }

    public void PlayPigDeath()
    {
        PigsSoundEffects.Play();
    }

    public void PlaySelectedBirdsSoundEffects(BirdType birdType)
    {
        SelectedBirdsSoundEffects.clip = SelectedSounds[(int)birdType];
        SelectedBirdsSoundEffects.Play();
    }
    
    public void PlayBirdLaunch(BirdType birdType)
    {
        BirdsSoundEffects.clip = BirdsSounds[(int)birdType];
        BirdsSoundEffects.Play();
    }
    
    public void PlaySpecialAbility(BirdType birdType)
    {
        BirdsSpecialAbilityEffects.clip = BirdsSpecialAbilitySounds[(int)birdType];
        BirdsSpecialAbilityEffects.Play();
    }

    public void PlayBirdDeath()
    {
        BirdDestroyed.Play();
    }

    public void PlayDestroyedBlock(BlockType blockType)
    {
        BlockDestroyed.clip = BlockDestroyedSounds[(int)blockType];
        BlockDestroyed.Play();
    }

    public void PlayEndLevel(bool isWin)
    {
        LevelEndMusic.clip = LevelEndClips[Convert.ToInt32(isWin)];
        LevelEndMusic.Play();
    }
}
