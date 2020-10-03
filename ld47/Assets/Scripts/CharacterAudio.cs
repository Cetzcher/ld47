using UnityEngine;

class CharacterAudio : MonoBehaviour 
{

    public AudioSource source;
    public AudioSource walkSource;

    public AudioClip jumpAudio;
    public AudioClip doubleJump;
    public AudioClip walking;

    private bool isPlayingWalk = false;

    public void PlaySingle(string what) {
        switch(what) 
        {
            case "walk":
                if(!isPlayingWalk) 
                    walkSource.Play();
                isPlayingWalk = true; 
                break;
            case "walk_end": 
            if(isPlayingWalk)
                walkSource.Stop(); 
            isPlayingWalk = false;    
            break;
            case "jump": source.PlayOneShot(jumpAudio); break;
            case "double_jump": source.PlayOneShot(doubleJump); break;
            default: Debug.LogWarning("cannot play audio"); break;
        }

    }

}