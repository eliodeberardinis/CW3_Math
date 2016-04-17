using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Player player;
    public Text scoreLabel;

    public void StartGame(int mode)
    {
        player.StartGame(mode);
        gameObject.SetActive(false);
       // Cursor.visible = false;
    }

    public void EndGame(float distanceTraveled)
    {
        scoreLabel.text = ((int)(distanceTraveled * 10f)).ToString();
        gameObject.SetActive(true);
        //Cursor.visible = false;
    }
}