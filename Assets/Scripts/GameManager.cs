using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool gameActive;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (player.health <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        gameActive = false;
    }
}
