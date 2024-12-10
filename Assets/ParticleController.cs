using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem mudParticle1;
    [SerializeField] ParticleSystem mudParticle2;
    [SerializeField] TrailRenderer boostTrail;
    [Range(0, 90)] [SerializeField] int mudOccurAfterVelocity;
    [Range(0, 90)] [SerializeField] int boostTrailAfterVelocity;
    [Range(0, 0.2f)] [SerializeField] float mudFormationPeriod;
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask River; // Layer for rivers
    [SerializeField] float detectionRadius = 5f; // Radius to detect rivers

    float counter;

    public int OccurAfterVelocity { get => mudOccurAfterVelocity; set => mudOccurAfterVelocity = value; }

    private ParticleSystem.MainModule mudParticle1Main;
    private ParticleSystem.MainModule mudParticle2Main;

    void Start()
    {
        // Cache the MainModule for better performance
        mudParticle1Main = mudParticle1.main;
        mudParticle2Main = mudParticle2.main;

        boostTrail.emitting = false;
    }

    private void Update()
    {
        counter += Time.deltaTime;

        if (Mathf.Abs(rb.velocity.x) > OccurAfterVelocity)
        {
            if (counter > mudFormationPeriod)
            {
                mudParticle1.Play();
                mudParticle2.Play();
                counter = 1;
            }
        }

        if (Mathf.Abs(rb.velocity.x) > boostTrailAfterVelocity)
        {
            boostTrail.emitting = true;
        }
        else
        {
            boostTrail.emitting = false;
        }

        // Check for overlap with river objects
        if (IsNearRiver())
        {
            SetParticleColor(new Color(0.647f, 0.906f, 1.0f, 1.0f));
        }
        else
        {
            SetParticleColor(new Color(0.227f, 0.078f, 0.0f, 1.0f));
        }
    }

    private bool IsNearRiver()
    {
        // Check if any objects in the river layer are within the detection radius
        Collider[] hitColliders = Physics.OverlapSphere(rb.transform.position, detectionRadius, River);
        return hitColliders.Length > 0;
    }

    private void SetParticleColor(Color color)
    {
        mudParticle1Main.startColor = color;
        mudParticle2Main.startColor = color;
    }
}
