using UnityEngine;

//Script to deal with collisions with obstacles, death, explosion and respawn of the character
public class Avatar : MonoBehaviour
{
    //Instances of the particle trail, the player object and the mesh of the spaceship
    public ParticleSystem trail, burst;
    public ParticleSystem shape;
    public MeshRenderer shipRender;

    //Instance of the player object (parent)
    private Player player;
    public float deathCountdown = -1f;

    //Initialize the plaer object
    private void Awake()
    {
        player = transform.root.GetComponent<Player>();
    }

    private void Update()
    {
        //while the player is alive show the particle trail and the ship activated.
        if (deathCountdown >= 0f)
        {
            deathCountdown -= Time.deltaTime;
            if (deathCountdown <= 0f)
            {
                deathCountdown = -1f;
                shipRender.enabled = true;
                trail.enableEmission = true;
                player.Die();
            }
        }
    }

    //Detect collision with an obstacle, make the ship "explode" and deactivate it
    private void OnTriggerEnter(Collider collider)
    {
        if (deathCountdown < 0f)
        {
            
            trail.enableEmission = false;
            shipRender.enabled = false;
            burst.Emit(burst.maxParticles);
            deathCountdown = burst.startLifetime;
        }
    }
}