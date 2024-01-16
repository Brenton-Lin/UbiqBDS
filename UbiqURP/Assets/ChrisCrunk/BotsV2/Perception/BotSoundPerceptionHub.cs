using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BotSoundPerceptionHub : MonoBehaviour
{
    [SerializeField]
    Dictionary<AudioSource, float> soundsAndVols;

    public int soundFidelity = 64;

    public float soundThreshold;

    public float updateStep = 0.1f;
    private float currentUpdateTime = 0f;

    public void Update()
    {
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            soundsAndVols = AuditAllSounds();
        }
    }

    public Dictionary<AudioSource, float> AuditAllSounds()
    {
        List<AudioSource> sources = new List<AudioSource>(FindObjectsOfType<AudioSource>());

        Dictionary<AudioSource, float> soundsWithVols = new Dictionary<AudioSource, float>();

        // every sound in the scene
        foreach (AudioSource source in sources)
        {
            soundsWithVols.Add(source, GetSoundVolume(source.timeSamples, source));
        }

        return soundsWithVols;
    }


    public float GetSoundVolume(int clipPosition, AudioSource sound)
    {

        int sampleSize = soundFidelity;

        int startPosition = clipPosition - sampleSize;

        if (!sound.isPlaying || clipPosition - sampleSize < 0)
        {
            // print("No sound playing");
            return 0;
        }


        float[] samples = new float[sampleSize];

        sound.clip.GetData(samples, startPosition);

        float volume = 0;

        for (int i = 0; i < soundFidelity; i++)
        {
            volume += Mathf.Abs(samples[i]);
        }

        volume /= soundFidelity;

        // audio mixer mod

/*        if (sound.TryGetComponent<AudioMixer>(out AudioMixer mixer))
        {
            if (mixer == null)
                return volume;

            if (sound.name == "ExplosionSource")
            {
                float scale;
                mixer.GetFloat("ExplosionVolume", out scale);

                scale = Mathf.Log10(scale);

                volume *= scale;
            }
        }*/


        return volume;
    }

    // get the sounds

    // store them with their volume
    
    // check bot's distance to sound and get best one

    public AudioSource GetMostNoticedSound(AiAgent bot)
    {
        AudioSource mostNoticedSound = null;
        float highestVolume = -1;

        float shotSound = 0;
        float plodSound = 0;



        foreach(KeyValuePair<AudioSource, float> pair in soundsAndVols)
        {

            float vol = GetSoundVolume(pair.Key.timeSamples, pair.Key);



            // Debug.Log("Volume of " + pair.Key.name + " is: " + vol);

            if (pair.Key.name == "ExplosionSource")
                plodSound = vol;

            if (pair.Key.name == "ShotSource")
                shotSound = vol;

            // get distance from bot to sound

            Vector3 soundLoc = pair.Key.gameObject.transform.position;
            Vector3 botPos = bot.gameObject.transform.position;
            float distanceFromSound = Vector3.Distance(soundLoc, botPos);

            // divide volume by distance for a scale effect

            if (vol / distanceFromSound < soundThreshold)
            {
                continue;
            }

            if (vol / distanceFromSound  > highestVolume) 
            {
                mostNoticedSound = pair.Key;
                highestVolume = vol / distanceFromSound;
            }

        }


        string s = null;
        if (shotSound > plodSound)
        {
            s = "Shot was loudest";
        }
        else
            s = "Explode was loudest";


        // Debug.Log("Most noticed sound: " + s);

        return mostNoticedSound;

    }
    
}
