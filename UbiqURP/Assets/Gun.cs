using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpread = new Vector3 (0.05f, 0.05f, 0.05f);
    [SerializeField]
    private ParticleSystem ShootEffect;
    [SerializeField]
    private ParticleSystem HitEffect;
    [SerializeField]
    private Transform FirePoint;
    [SerializeField]
    private TrailRenderer Tracer;
    [SerializeField]
    private float ShootDelay;
    [SerializeField]
    private LayerMask Mask;



    private float LastShootTime;


    private void Awake()
    {
        //For eventual
        //Animator = GetComponent<Animator>();
    }

    public void shoot()
    {
        if(LastShootTime + ShootDelay < Time.time)
        {
            //Animator.SetBool("IsShooting", true);
            ShootEffect.Play();
            Vector3 direction = GetDirection();

            if(Physics.Raycast(FirePoint.position,direction,out RaycastHit hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(Tracer, FirePoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit));

                LastShootTime = Time.time;
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpread.x, BulletSpread.x),
                Random.Range(-BulletSpread.y, BulletSpread.y),
                Random.Range(-BulletSpread.z, BulletSpread.z)
             );
            direction.Normalize();
        }
        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while(time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        //Animator.SetBool("IsShooting", false);
        Trail.transform.position = Hit.point;
        Instantiate(HitEffect, Hit.point, Quaternion.LookRotation(Hit.normal));

        Destroy(Trail.gameObject, Trail.time);
    }

}
