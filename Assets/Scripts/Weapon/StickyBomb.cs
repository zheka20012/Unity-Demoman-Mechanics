using UnityEngine;
using System.Collections;

public class StickyBomb : MonoBehaviour
{
    [SerializeField]
    private Rigidbody Rigidbody;
    [SerializeField]
    private ParticleSystem ExplosionParticles;
    [SerializeField]
    private AudioSource ExplosionSource;
    [SerializeField]
    private float ExplosionRadius = 2.77f;
    [SerializeField]
    private AnimationCurve DamageFalloff;
    [SerializeField]
    private float MaxDamage = 144f;
    [SerializeField]
    private float ExplosionForce = 10f;

    private float ActivationTimer = 0f;


    
    public void Throw(float ActivationTime)
    {
        ActivationTimer = ActivationTime;
    }

    public void Explode()
    {
        if (ActivationTimer >= 0) return;
        
        Collider[] affectedColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        Rigidbody rb;

        ExplosionParticles.transform.parent = null;
        ExplosionParticles.Play();
        ExplosionSource.Play();

        foreach (var collider in affectedColliders)
        {
            if (collider.GetComponent<IDamageable>() != null)
            {
                Vector3 direction = (collider.transform.position - transform.position).normalized;
                float damage =  DamageFalloff.Evaluate(1/direction.magnitude) * MaxDamage;
                collider.GetComponent<IDamageable>().TakeDamage(damage);
            }

            rb = collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, 2, ForceMode.Impulse);
            }

            rb = null;
        }

        Destroy(gameObject);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if(ActivationTimer < 0) return;

        ActivationTimer -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")) return;

        Destroy(Rigidbody);
        Destroy(GetComponent<Collider>());

    }


}
