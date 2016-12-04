using UnityEngine;
using UnityEngine.UI;

//Script for the Main Menu of the game
public class MainMenu : MonoBehaviour
{
    //Instances of the player object and the text element 
    public Player player;
    public Text scoreLabel;

    //Deactivates itself once the player has decided the mode to play (Easy, Medium, Hard)
    public void StartGame(int mode)
    {
        player.StartGame(mode);
        gameObject.SetActive(false);
       // Cursor.visible = false;
    }

    //Display the score after the player dies and reactivates its function: Play Again Scenario
    public void EndGame(float distanceTraveled)
    {
        scoreLabel.text = ((int)(distanceTraveled * 10f)).ToString();
        gameObject.SetActive(true);
        //Cursor.visible = false;
    }

}