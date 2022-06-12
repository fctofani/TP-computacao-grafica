using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject gameOver;
    public float score = 0;
    public int difficulty;

    public Text scoreText;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isDead) {
            score += Time.deltaTime * (player.speed / (10 - difficulty));
            scoreText.text = Mathf.RoundToInt(score).ToString();
        }
    }

    public void ShowGameOver() {
        gameOver.SetActive(true);
    }
}
