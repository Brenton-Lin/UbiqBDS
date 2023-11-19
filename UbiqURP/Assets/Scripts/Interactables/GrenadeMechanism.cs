using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeMechanism : MonoBehaviour
{
    public ParticleSystem explosion;
    public float fuse;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public IEnumerator explode()
    {
        yield return new WaitForSeconds(fuse);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
