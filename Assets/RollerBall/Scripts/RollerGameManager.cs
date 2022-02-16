using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RollerGameManager : Singleton<RollerGameManager>
{
	enum State
	{
		TITLE,
		PLAYER_START,
		GAME,
		PLAYER_DEAD,
		GAME_OVER,
		GAME_WON
	}

	[SerializeField] GameObject playerPrefab;
	[SerializeField] Transform playerSpawn;
	[SerializeField] GameObject mainCamera;

	[SerializeField] GameObject titleScreen;
	[SerializeField] GameObject gameOverScreen;
	[SerializeField] GameObject gameWonScreen;
	[SerializeField] TMP_Text scoreUI;
	[SerializeField] TMP_Text livesUI;
	[SerializeField] TMP_Text timeUI;
	[SerializeField] Slider healthBarUi;

	public float playerHealth { set { healthBarUi.value = value; } }

	public delegate void GameEvent();

	public event GameEvent startGameEvent;
	public event GameEvent stopGameEvent;

	int score = 0;
	int lives = 0;
	State state = State.TITLE;
	float stateTimer;
	float gameTime = 0;

	public int Score
	{
		get { return score; }
		set
		{
			score = value;
			scoreUI.text = score.ToString("D1");
		}
	}
	public float GameTime
	{
		get { return gameTime; }
		set
		{
			gameTime = value;
			timeUI.text = "<mspace=mspace=36>" + gameTime.ToString("0.0") + "</mspace>";
		}
	}
	public int Lives
	{
		get { return lives; }
		set
		{
			lives = value;
			livesUI.text = "Lives: " + lives.ToString();
		}
	}

    private void Update()
    {
		stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.TITLE:
                break;
            case State.PLAYER_START:
				DestroyAllEnemies();
				Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
				mainCamera.SetActive(false);
				startGameEvent?.Invoke();
				GameTime = 60;

				state = State.GAME;
				break;
            case State.GAME:
				GameTime -= Time.deltaTime;
				if (gameTime <= 0)
                {
					GameTime = 0;
					state = State.GAME_OVER;
					stateTimer = 5;
                }
				if (score == 3)
                {
					state = State.GAME_WON;
					gameWonScreen.SetActive(true);
					stateTimer = 10;
				}
                break;
			case State.PLAYER_DEAD:
				if (stateTimer <= 0)
                {
					state = State.PLAYER_START;
                }
                break;
            case State.GAME_OVER:
				if (stateTimer <= 0)
                {
					state = State.TITLE;
					gameOverScreen.SetActive(false);
					titleScreen.SetActive(true);
                }
                break;
			case State.GAME_WON:
				if (stateTimer <= 0)
				{
					state = State.TITLE;
					gameWonScreen.SetActive(false);
					titleScreen.SetActive(true);
				}
				break;
			default:
                break;
        }
    }
    public void OnStartGame()
	{
		state = State.PLAYER_START;
		Score = 0;
		Lives = 3;
		gameTime = 0;

		titleScreen.SetActive(false);

	}

	public void OnStartTitle()
	{
		state = State.TITLE;
		titleScreen.SetActive(true);
		stopGameEvent?.Invoke();
	}

	public void OnPlayerDead()
	{
		mainCamera.SetActive(true);

		Lives -= 1;
		if (Lives == 0)
        {
			state = State.GAME_OVER;
			stateTimer = 5;

			gameOverScreen.SetActive(true);
        } else
        {
			state = State.PLAYER_DEAD;
			stateTimer = 3;
        }
		stopGameEvent?.Invoke();
		
	}

	private void DestroyAllEnemies()
	{
		// destroy all enemies
/*		SpaceEnemy[] spaceEnemies = FindObjectsOfType<SpaceEnemy>();
		foreach (var spaceEnemy in spaceEnemies)
		{
			Destroy(spaceEnemy.gameObject);
		}*/
	}

}
