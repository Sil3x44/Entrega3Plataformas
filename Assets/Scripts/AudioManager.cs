using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;

    [Header("Music")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip villageMusic;
    [SerializeField] private AudioClip caveMusic;
    [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip victoryMusic;

    [Header("Player Sounds")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip doubleJumpSound;
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private AudioClip hitEnemySound;
    [SerializeField] private AudioClip playerDamageSound;

    [Header("Enemy Sounds")]
    [SerializeField] private AudioClip batChargeSound;
    [SerializeField] private AudioClip bossChargeSound;
    [SerializeField] private AudioClip alertSound;

    [Header("Interaction Sounds")]
    [SerializeField] private AudioClip npcInteractSound;
    [SerializeField] private AudioClip signInteractSound;
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private AudioClip itemPickupSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            PlayMusic(mainMenuMusic);
        }
        else if (scene.name == "Village")
        {
            PlayMusic(villageMusic);
        }
        else if (scene.name == "Cave")
        {
            PlayMusic(caveMusic);
        }
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null) return;
        if (musicSource.clip == musicClip) return;

        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayGameOverMusic()
    {
        PlayMusic(gameOverMusic);
    }

    public void PlayVictoryMusic()
    {
        PlayMusic(victoryMusic);
    }

    public void PlaySound(AudioClip soundClip)
    {
        if (soundClip == null) return;

        soundEffectSource.PlayOneShot(soundClip);
    }

    public void PlayJumpSound() { PlaySound(jumpSound); }
    public void PlayDoubleJumpSound() { PlaySound(doubleJumpSound); }
    public void PlayFootstepSound() { PlaySound(footstepSound); }
    public void PlayHitEnemySound() { PlaySound(hitEnemySound); }
    public void PlayPlayerDamageSound() { PlaySound(playerDamageSound); }
    public void PlayBatChargeSound() { PlaySound(batChargeSound); }
    public void PlayBossChargeSound() { PlaySound(bossChargeSound); }
    public void PlayAlertSound() { PlaySound(alertSound); }
    public void PlayNpcInteractSound() { PlaySound(npcInteractSound); }
    public void PlaySignInteractSound() { PlaySound(signInteractSound); }
    public void PlayCoinPickupSound() { PlaySound(coinPickupSound); }
    public void PlayItemPickupSound() { PlaySound(itemPickupSound); }
}
