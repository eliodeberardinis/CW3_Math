using UnityEngine;

public class Player : MonoBehaviour
{
    //References to the pipy system and the pipe object
    public PipeSystem pipeSystem;
    private Pipe currentPipe;

    //Variables to keep track of the system's rotation, distance travelled and the player's rotation
    private float distanceTraveled;
    private float deltaToRotation;
    private float systemRotation;

    //References to the world and the rotater object
    private Transform world, rotater;

    //Variables to keep track of world rotation and the velocity
    private float worldRotation, avatarRotation;
    public float rotationVelocity;

    //Reference to the main menu
    public MainMenu mainMenu;

    //Gameplay elements (adjust the velocity, acceleration and their initial values)
    public float startVelocity;
    public float[] accelerations;
    private float acceleration, velocity;

    //Reference to the HUD displaying current distance travelled and speed
    public HUD hud;

    //Initialization
    private void Awake()
    {
        world = pipeSystem.transform.parent; //Set the pipe system to th world
        rotater = transform.GetChild(0);     //Obtain an istanc eof the rotater of the player
        gameObject.SetActive(false);        //Disable the player before the player has set his mode of play (Easy, Medium, Hard)
    }

    public void StartGame(int accelerationMode)
    {
        distanceTraveled = 0f; //Initializing parameters
        avatarRotation = 0f;
        systemRotation = 0f;
        worldRotation = 0f;
        acceleration = accelerations[accelerationMode]; //Get the current choice of velocity (Easy, Medium, Hard)
        velocity = startVelocity;
        currentPipe = pipeSystem.SetupFirstPipe();   //Stat generating the pipe system
        SetupCurrentPipe();
        gameObject.SetActive(true);    //Activate the player to start the game

        hud.SetValues(distanceTraveled, velocity);  //Send the initial values to the HUD
    }

    //update the systems stats every frame
    private void Update()
    {
        velocity += acceleration * Time.deltaTime; //update acceleration
        float delta = velocity * Time.deltaTime;
        distanceTraveled += delta;
        systemRotation += delta * deltaToRotation; //update rotation

        if (systemRotation >= currentPipe.CurveAngle) //When the length of the current pipe has been reached, set up a new one and adjust the rotation
        {
            delta = (systemRotation - currentPipe.CurveAngle) / deltaToRotation;
            currentPipe = pipeSystem.SetupNextPipe();
            SetupCurrentPipe();
            systemRotation = delta * deltaToRotation;
        }

        pipeSystem.transform.localRotation = Quaternion.Euler(0f, 0f, systemRotation); //Update the playe'rs rotation according to the new pipe
        UpdateAvatarRotation();
        hud.SetValues(distanceTraveled, velocity);

    }

    //Takes in the user input and rotates the system to simulate rotating the player's avatar
    private void UpdateAvatarRotation()
    {
        avatarRotation +=
            rotationVelocity * Time.deltaTime * Input.GetAxis("Horizontal");
        if (avatarRotation < 0f)
        {
            avatarRotation += 360f;
        }
        else if (avatarRotation >= 360f)
        {
            avatarRotation -= 360f;
        }
        rotater.localRotation = Quaternion.Euler(avatarRotation, 0f, 0f);
    }

    //Translates a curved distance (in rad) into a regular distance into metrical units (although not explicit)
    private void SetupCurrentPipe()
    {
        deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
        worldRotation += currentPipe.RelativeRotation;
        if (worldRotation < 0f)
        {
            worldRotation += 360f;
        }
        else if (worldRotation >= 360f)
        {
            worldRotation -= 360f;
        }
        world.localRotation = Quaternion.Euler(worldRotation, 0f, 0f);
    }

    //Make the player explode when an object is hit. Pass the current distance travelled to the main manu to display a score
    public void Die()
    {
        mainMenu.EndGame(distanceTraveled);
        gameObject.SetActive(false);
    }
}