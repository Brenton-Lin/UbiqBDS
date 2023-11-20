using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Magazine : MonoBehaviour
{

    public UnityEvent LockMag = new UnityEvent();


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<MagazineLockTrigger>()) 
        {
            Debug.Log("Collided with the MagWellLock");
            other.GetComponent<MagazineLockTrigger>().gun.GetComponent<RifleAndSnapReload>().AddMagToGun();
            LockMag.Invoke();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("UnCollided");
        if (other.GetComponent<MagazineLockTrigger>())
        {
            other.GetComponent<MagazineLockTrigger>().gun.GetComponent<RifleAndSnapReload>().RemoveMagFromGun();
        }
    }
}
