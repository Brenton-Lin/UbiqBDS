using System.Collections;
using System.Collections.Generic;
using Tilia.Interactions.Interactables.Interactors;
using UnityEngine;
using UnityEngine.Events;

public class ReloadingRifle : SimpleGun
{
    public GameObject rifle;
    public GameObject magazine;
    public RealisticMagazinePath magazineWell;

    private bool magInGun = true;
    [SerializeField]
    private bool magRemoved = false;

    public UnityEvent LockInMag = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        // magazine interactible follows gun interactible without nesting it
        if (magInGun)
        {
            Debug.Log("Mag in gun");
            magazine.transform.position = rifle.transform.position;
            magazine.transform.rotation = rifle.transform.rotation * Quaternion.Euler(0, 90, 0);
        }

        if (magazineWell.magazineInserted) // true on Start
        {
            magInGun = true;
        }
        else
        {
            magInGun = false;
            magRemoved = true;
        }

        if (magRemoved && magazineWell.magazineInserted)
        {
            // using InteractorFacade UnGrabAll();
            LockInMag.Invoke();
            magInGun = true;
            magRemoved = false;
        }



        // if mag in gun, mag's in gun
        // mag in gun when MagazinePath is true

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




}
