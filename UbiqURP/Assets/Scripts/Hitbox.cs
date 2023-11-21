using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public BotHealth health;
    // Start is called before the first frame update
    public void OnRaycastHit(SimpleGun gun)
    {
        health.DoDamage(gun.damage);
    }
}
