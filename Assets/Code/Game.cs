using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	private static readonly float respawnTime = 2f;
	private static readonly float capturePointPointsPerSecond = 1f;
	private static readonly float pointsNeededToWin = 25f;

	#region Editor fields
	public List<Map> mapList;
	public GameObject ammoPickUpPrefab;
	public List<Pickup> possiblePickups;
	public PlayerController playerPrefab;
	#endregion

	#region Fields
	Map currentMap;

	float redPoints;
	float bluePoints;

	public List<PlayerController> players { get; set; }

	public static Game game;
	#endregion
	
	#region Properties
	public float BluePoints { get { return bluePoints; } set { bluePoints = value; if(BluePoints >= pointsNeededToWin) GameIsOver(GridCreator.collisionLayer.BLUE);} }
	public float RedPoints { get { return redPoints; } set { redPoints = value; if(RedPoints >= pointsNeededToWin) GameIsOver(GridCreator.collisionLayer.RED);} }
	#endregion

	public delegate void GameOverHandler(GridCreator.collisionLayer winner);
	public event GameOverHandler GameOver;

	#region Initilization
	void Awake () {
		game = this;

		// 1 choose map and spawn
		Map chosenMap = mapList[Random.Range(0, mapList.Count)];
		currentMap = Instantiate(chosenMap, Vector3.zero, Quaternion.identity) as Map;
		
		// 2 spawnplayers
		players = new List<PlayerController>();
		
		foreach(int playerNumber in PlayerManager.Instance.JoinedPlayers)
		{
			SpawnPlayer(CreatePlayer(playerNumber));
		}
	}

	void Start () {

	}
	#endregion

	#region Methods
	// Update is called once per frame
	void Update () {
		foreach(CapturePoint point in currentMap.capturePoints) {
			if(point.Owner == GridCreator.collisionLayer.BLUE)
				BluePoints += capturePointPointsPerSecond * Time.deltaTime;
			else if(point.Owner == GridCreator.collisionLayer.RED)
				RedPoints += capturePointPointsPerSecond * Time.deltaTime;
		}
	}

	void GameIsOver (GridCreator.collisionLayer teamThatWon)
	{
		foreach(PlayerController p in players)
			Destroy(p.gameObject);

		if(GameOver != null)
			GameOver(teamThatWon);

		StartCoroutine(LoadMenu());
	}

	IEnumerator LoadMenu() {
		yield return new WaitForSeconds(2f);

		PlayerManager.Instance.Reset();
		Application.LoadLevel("menu");
	}

	PlayerController CreatePlayer (int playerNumber)
	{
		PlayerController player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as PlayerController;
		player.PlayerNumber = playerNumber;
		player.PlayerDead += HandlePlayerDead;
		player.gameObject.SetActive(false);

		if(players.Count %2 == 0)
			player.playerColor = GridCreator.collisionLayer.BLUE;
		else
			player.playerColor = GridCreator.collisionLayer.RED;

		players.Add(player);

		return player;
	}

	void SpawnPlayer (PlayerController player)
	{
		player.HP = 1f;
		player.gameObject.SetActive(true);
		player.enabled = true;
		player.animator.SetTrigger("Spawn");
		player.transform.position = currentMap.GetSpawnPoint().position;
		player.Ammo = 500;
		player.reset();
	}

	IEnumerator SpawnPlayerWithDelay (PlayerController player, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		SpawnPlayer(player);
	}

	void HandlePlayerDead (PlayerController player)
	{
		currentMap.mapCamera.ShakeCamera(2, .5f);

		spawnPickups( player );

		player.enabled = false;

		StartCoroutine(SpawnPlayerWithDelay(player, respawnTime));
	}

	void spawnPickups( PlayerController player )
	{
		// Leave a pickup at random together with an ammo pickup.
		Instantiate( ammoPickUpPrefab, player.transform.position, Quaternion.identity );
		
		float rand_pickup = Random.value;
		if( rand_pickup >= 0.05f )
		{
			rand_pickup = Random.value;
			float number_pickups = (float)possiblePickups.Count;
			// rand_pickup is between 0.0 and 1.0, we need between 0 and number_pickups (=2 - 1 = 1 = either 0 or 1)
			rand_pickup *= 100;
			rand_pickup /= (100 / number_pickups);  
			int index = Mathf.Clamp((int)rand_pickup, 0, (int)number_pickups - 1);
			Debug.Log( "The index of the pickup is: " + index );
			Instantiate( possiblePickups[index], player.transform.position + Vector3.right, Quaternion.identity );
		}
	}
	#endregion
}
