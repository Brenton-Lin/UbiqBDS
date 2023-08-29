
using UnityEngine;
using UnityEngine.UI;

public class SimpleGun : MonoBehaviour
{
    // Start is called before the first frame update

    public float range = 100f;
    public float damage = 1f;
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
            if (Physics.Raycast(FirePoint.position, direction, out RaycastHit hit, float.MaxValue, Mask))
            {
                Debug.Log(hit.transform.name);
                //Instantiate a HitEffect facing the user.
                Instantiate(HitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                TrackedShootingTarget target = hit.transform.GetComponent<TrackedShootingTarget>();

                if(target != null)
                {
                    target.HitTarget();
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
