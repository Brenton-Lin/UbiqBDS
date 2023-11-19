using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RifleAndSnapReload : SimpleGun
{
    public GameObject rifle;

    private bool magInGun = false;

    

    // Start is called before the first frame update
    void Start()
    {
        rifle = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShootRifle()
    {
        if (magInGun)
        {
            Shoot();
        }
    }


    // called on Magazine.Interactible IsGrabbed Event
    public void RemoveMagFromGun()
    {
        magInGun = false;
    }

    public void AddMagToGun()
    {
        magInGun = true;
    }
}
