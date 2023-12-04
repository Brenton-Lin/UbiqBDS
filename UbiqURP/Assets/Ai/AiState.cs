using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateId
{
    ChasePlayer,
    Relaxed,
    Alert,
    PeekShoot,
    BlindShoot,
    
    StartFormation,
    TakeCover,
    ShootFromCover,
    LeaderCommunication,
    BravoHorseshoe

}

// if not suppressed, enemies that notice you shooting a comrade will switch attention to you

public interface AiState
{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
}
