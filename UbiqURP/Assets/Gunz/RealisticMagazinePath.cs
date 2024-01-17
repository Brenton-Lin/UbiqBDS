using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealisticMagazinePath : MonoBehaviour
{
    // Start is called before the first frame update

    public bool magazineInserted = true;

    // check order that triggers are entered by magazine to determine direction of magazine insertion
    public GameObject magazinePath1;
    public GameObject magazinePath2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collided");
        if (other.name == "MagazineTriggerForGunInterfacing") // maybe will give magazine its own component
        {
            //Debug.Log("CollidedAsMagazine");
            magazineInserted = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("UnCollided");
        if (other.name == "MagazineTriggerForGunInterfacing")
        {
            magazineInserted = false;
        }
    }
}
