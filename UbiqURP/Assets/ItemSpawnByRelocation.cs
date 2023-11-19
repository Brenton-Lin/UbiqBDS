using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnByRelocation : MonoBehaviour
{
    private GameObject spawnZone;
    public GameObject item;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForInstantiateThenSpawn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator WaitForInstantiateThenSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        spawnZone = GameObject.FindGameObjectsWithTag("SnapZoneSpawn")[0];
        Debug.Log("spawn zone pos: " + spawnZone.transform.position);
        Debug.Log("Spawn zone tag object name: " + spawnZone.name);
        item.transform.position = spawnZone.transform.position;

        yield return null;
    }

    // get item
    // get snapZone by tag

    // on start tp item to snapZone

}
