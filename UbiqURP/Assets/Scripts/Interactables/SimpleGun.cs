
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SimpleGun : MonoBehaviour
{
    // Start is called before the first frame update

    public float range = 100f;
    public float damage = 20f;
    public float fireRate = 1f;
    public float impactForce = 20f;

    [SerializeField]
    private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpread = new Vector3(0.05f, 0.05f, 0.05f);
    [SerializeField]
    private ParticleSystem ShootEffect;
    [SerializeField]
    private ParticleSystem HitEffect;
    [SerializeField]
    private Transform FirePoint;
    [SerializeField]
    private TrailRenderer Tracer;
    [SerializeField]
    private float timeToNext = 0f;
    [SerializeField]
    private LayerMask Mask;



    private float LastShootTime;
    public void Shoot()
    {
        if (Time.time >= timeToNext)
        {
            ShootEffect.Play();
            timeToNext = Time.time + 1f / fireRate;
            Vector3 direction = GetDirection();

            if (Physics.Raycast(FirePoint.position, direction, out RaycastHit hit))
            {
                Debug.Log("hit: " + hit.transform.name);
                //Instantiate a HitEffect facing the user.
                Instantiate(HitEffect, hit.point, Quaternion.LookRotation(hit.normal));

                //updating target trackers
                TrackedShootingTarget target = hit.transform.GetComponent<TrackedShootingTarget>();
                if(target)
                {
                    target.HitTarget();
                }

                //doing damage
                var hitBox = hit.transform.GetComponent<Hitbox>();
                if(hitBox)
                {
                    hitBox.OnRaycastHit(this);
                }
            }
        }
    }

    public void ShootBotLogic()
    {
        if (Time.time >= timeToNext)
        {
            ShootEffect.Play();
            timeToNext = Time.time + 1f / fireRate;
            Vector3 direction = GetDirection();

            RaycastHit[] hits;
            hits = Physics.RaycastAll(FirePoint.position, direction);

            bool passedThroughSphere = false;
            foreach (RaycastHit hit in hits.Reverse())
            {
                if (hit.collider.tag == "SuppressionSphere")
                {
                    hit.transform.SendMessage("HitByRay");
                    Debug.Log("Suppressing");
                    passedThroughSphere = true;
                }
                if (hit.collider.tag != "SuppressionSphere")
                {
                    Debug.Log("hit: " + hit.transform.name);

                    //Instantiate a HitEffect facing the user.
                    Instantiate(HitEffect, hit.point, Quaternion.LookRotation(hit.normal));

                    //updating target trackers
                    TrackedShootingTarget target = hit.transform.GetComponent<TrackedShootingTarget>();
                    if (target)
                    {
                        target.HitTarget();
                    }

                    //doing damage
                    var hitBox = hit.transform.GetComponent<Hitbox>();
                    if (hitBox)
                    {
                        hitBox.OnRaycastHit(this);
                    }

                    break;
                }

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
}
