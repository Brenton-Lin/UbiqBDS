using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.AI;
using Ubiq.Messaging;

public class AiChasePlayerState : AiState
{
    public LocalBotControl bot;

    // just using this for the demo, way better ways to do it later.
    public float detectionDistance = 5.0f;
    public void Enter(AiAgent agent)
    {
        bot = GameObject.Find("Bots/Y Bot").GetComponent<LocalBotControl>();
        
        bot.dead = false;
    }

    public void Exit(AiAgent agent)
    {
        if(bot != null)
            bot.dead = true;
    }

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }


    public void Update(AiAgent agent)
    {

    }
}
