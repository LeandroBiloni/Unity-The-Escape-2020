using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private AudioSource musicSource;
	private AudioSource SFXSource;
	private AudioSource walkingSource;
	public AudioClip walkingSound;

	private void Awake() // agrega los audio source al audio manager que es un empty con este script que tiene que estar en la escena
	{
		musicSource = gameObject.AddComponent<AudioSource>();
		SFXSource = gameObject.AddComponent<AudioSource>();
		walkingSource = gameObject.AddComponent<AudioSource>();

		musicSource.loop = true;
		walkingSource.loop = true;
		walkingSource.Play();
		walkingSource.Pause();
		walkingSource.clip = walkingSound;
	}

	public void PlayMusic(AudioClip musicClip) // Le pasas la musica a traves de otros scritps referenciando el AudioManager y el clip
	{
		musicSource.volume = 1f;
		musicSource.clip = musicClip;
		musicSource.Play();
	}

	public void PlayMusic(AudioClip musicClip, float volume) // Le pasas la musica a traves de otros scritps referenciando el AudioManager y el clip
	{
		musicSource.clip = musicClip;
		musicSource.volume = volume;
		musicSource.Play();
	}

	public void PlaySFX(AudioClip clip) // Le pasas los Sound Effects a traves de otros scritps referenciando el AudioManager y el clip 
	{
		SFXSource.PlayOneShot(clip);
	}

	public void PlaySFX(AudioClip clip, float volume) // lo mismo que el de arriva + poder alterar el volumen con un float entre 0(mute/min) y 1(normal/max)
	{
		SFXSource.PlayOneShot(clip, volume);
	}

	public void WalkingSound(Vector3 velocity)
	{

		if(velocity != Vector3.zero)
		{
			walkingSource.UnPause();
		}
		else
		{
			walkingSource.Pause();
				
		}
	}

	public void StopSFX(AudioClip clip)
	{
		if (SFXSource.clip == clip)
			SFXSource.Stop();
	}
}
