using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RifleAndSnapReload : SimpleGun
{
    public GameObject rifle;

    public bool magInGun = false;
    public bool use;
    

    // Start is called before the first frame update
    void Start()
    {
        rifle = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        use = false;
    }

    public void ShootRifle()
    {
        Debug.Log("Trigger pulled");
        if (magInGun)
        {
            Debug.Log("Shot");
            Shoot();
            use = true;
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
