using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAudit : MonoBehaviour
{
    // stores stats on all the bots in the scene

    public int numBots;
    public List<AiAgent> bots;

    private void Start()
    {
        bots = new List<AiAgent>(FindObjectsOfType<AiAgent>());
        numBots = bots.Count;
    }

}
