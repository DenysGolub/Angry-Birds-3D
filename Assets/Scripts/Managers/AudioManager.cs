using UnityEngine;

public enum Bird
{
    Red,
    Blue, 
    Yellow,
    Black,
    White
}

public class AudioManager : MonoBehaviour
{
    [Header("Music")] 
    public AudioSource TitleMusic;

    public AudioSource GameMusic;
    
    [Header("Birds Sound Effects")]
    public AudioSource BirdsSoundEffects;
    public AudioClip[] BirdsSounds;

    [Header("Slingshot Sound Effects")] 
    public AudioSource SlingshotStrech;

    public AudioSource SlingshotLaunch;
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
    
    public void PlaySlingshotStretch()
    {
        SlingshotStrech.Play();
    }

    public void PlayLaunchSlingshot()
    {
        SlingshotLaunch.Play();
    }

    public void PlayBirdLaunch(Bird birdType)
    {
        BirdsSoundEffects.clip = BirdsSounds[(int)birdType];
        BirdsSoundEffects.Play();
    }
}
