using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AiStateId
{
    Calibration,
    Patrol,
    Guard,
    Investigate,

    Combat,
        ShootTarget, // branch if for in cover or not in cover
        TakeCover,  // find nearest object between danger(s) and self
        BlindFire, // shoot erratically
        GetSmall, // cower
        Flee, // when separated from main group


    MessageComrades,
    Hide,

    EnterFormation, // find several bots of the same team and form a shape based on their Role tag
    
    LeaderCommunication, // receives information from Platoon or T-Leader to advance to a location
                         // MessageComrades can incorporate this too
    // BravoHorseshoe,

    Move,
    Hurt,
    Die,

    ChasePlayer,
    Relaxed,
}


// if not suppressed, enemies that notice you shooting a comrade will switch attention to you

public interface AiState
{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);

    void GetNetworkUpdates(AiAgent agent);
}
