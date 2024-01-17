using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSenses: MonoBehaviour
{
    public AiEyes eyes;

    public AiEyes senses;
    public int scanFrequency = 30;

    public AiEars ears;

    public float hearingSensitivity;

    public List<AudioSource> heardSounds;
    public AudioSource mostNoticedSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
