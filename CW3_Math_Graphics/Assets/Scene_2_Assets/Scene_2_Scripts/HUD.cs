using UnityEngine;
using UnityEngine.UI;

//Script to provide visual information on the player's stats in game
public class HUD : MonoBehaviour
{
    //Instances of the text to display in game
    public Text distanceLabel, velocityLabel;

    //Updating the values as the player "travels through the pipes"
    public void SetValues(float distanceTraveled, float velocity)
    {
        distanceLabel.text = ((int)(distanceTraveled * 10f)).ToString();
        velocityLabel.text = ((int)(velocity * 10f)).ToString();
    }
}