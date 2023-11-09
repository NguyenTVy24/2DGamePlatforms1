using UnityEngine;

public class AudioManage : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource SFXSourceLongTime;
    public AudioClip[] audioClips;
    void  Awake(){
        musicSource.clip = audioClips[0];
        musicSource.Play();
    }
    public void PlayeSFX(AudioClip clip){
        SFXSource.PlayOneShot(clip);
    }
    public void PlaySFXLongTime(AudioClip clip,bool Open){
        SFXSourceLongTime.clip = clip;
        if(Open){
            SFXSourceLongTime.Play();
        }else{
            SFXSourceLongTime.Pause();
        }
    }
    public void SetMusic(AudioClip clip){
        musicSource.clip = clip;
        musicSource.Play();
    }
    public void SetMusicDefall(){
        musicSource.clip = audioClips[0];
        musicSource.Play();
    }
}
