using UnityEngine;

public class Avatar : MonoBehaviour
{

    public ParticleSystem trail, burst;
    public ParticleSystem shape;
    public MeshRenderer shipRender;

    private Player player;
    public float deathCountdown = -1f;

    private void Awake()
    {
        player = transform.root.GetComponent<Player>();
    }

    private void Update()
    {
        if (deathCountdown >= 0f)
        {
            deathCountdown -= Time.deltaTime;
            if (deathCountdown <= 0f)
            {
                deathCountdown = -1f;
                //shape.enableEmission = true;
                shipRender.enabled = true;
                trail.enableEmission = true;
                player.Die();
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (deathCountdown < 0f)
        {
            //shape.enableEmission = false;
            trail.enableEmission = false;
            shipRender.enabled = false;
            burst.Emit(burst.maxParticles);
            deathCountdown = burst.startLifetime;
        }
    }
}