using EazyTools.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Level[] levels;

    public PlayerCustomizationData playerData = null;
    HighscoresDemo highscoresDemo;

    int currentLevel = 0;

    //events
    [HideInInspector]
    public UnityEvent SomeoneDiedEvent;

    public RobotController[] players;

    public AudioClip gameplayMusic;
    public float volume = 1f;

    public EndGameManager endGameManager;

    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));
            }
            return _instance;
        }
    }

    [System.Serializable]
    public struct Level
    {
        public GameObject levelObj;
        public GameObject cameraPosition;
        public GameObject[] spawnPoints;
    }

    void Start()
    {
        SoundManager.PlayMusic(gameplayMusic, volume, true, true, 2f, 1f);

        playerData = GameObject.FindObjectOfType<PlayerCustomizationData>();
        highscoresDemo = GameObject.FindObjectOfType<HighscoresDemo>();

        //create event for lost connection
        if (SomeoneDiedEvent == null)
        {
            SomeoneDiedEvent = new UnityEvent();
        }
        //add listener for lost Connection ations
        SomeoneDiedEvent.AddListener(SomeoneDied);

        GoToLevel(0, false, true);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    NextLevel(false, false);
    } 

    public static void GoToLevel(int levelID, bool repositionimidiatetly, bool lookimidiatetly)
    {
        if (levelID > instance.levels.Length - 1)
        {
            return;
        }

        instance.currentLevel = levelID;
        Level level = instance.levels[levelID];

        CameraController camera = GetCamera();
        camera.SetMidpoint(level.levelObj.transform.position, lookimidiatetly);
        camera.SetTargetSitPosition(level.cameraPosition.transform.position, repositionimidiatetly);
    }

    public static void NextLevel(bool repositionimidiatetly, bool lookimidiatetly)
    {
        GoToLevel(instance.currentLevel + 1, repositionimidiatetly, lookimidiatetly);
    }

    public static CameraController GetCamera()
    {
        return Camera.main.GetComponent<CameraController>();
    }

    public static List<RobotController> GetPlayers()
    {
        GameObject[] playersObjs = GameObject.FindGameObjectsWithTag(GameRepository.playerTag);
        List<RobotController> players = new List<RobotController>();
        foreach (GameObject playerObj in playersObjs)
        {
            RobotController player = playerObj.GetComponent<RobotController>();
            if (player.isAlive)
            {
                players.Add(player);
            }
        }

        return players;
    }

    private void SomeoneDied()
    {
        Debug.Log(instance.levels.Length-1 + " <= " + instance.currentLevel);
        if ((instance.levels.Length-1) <= instance.currentLevel)
        {
            SendScoresToAzure();
            //finish game
            endGameManager.DoFadeOut();
            return;
        }

        int j = 0;
        for(int i=0;i< players.Length;i++)
        {
            if (players[i].isAlive)
            {
                players[i].gameObject.transform.position = levels[currentLevel + 1].spawnPoints[j].transform.position;
                players[i].gameObject.transform.rotation = levels[currentLevel + 1].spawnPoints[j].transform.rotation;
                j++;
                if(j>= levels[currentLevel + 1].spawnPoints.Length)
                {
                    break;
                }
            }
        }

        //go to next level camera
        NextLevel(false, false);
    }

    public static void SendScoresToAzure()
    {
        Debug.Log("empikame");
        for (int i = 0; i <= 3; i++)
        {
            PlayerCustomizationData playerData = null;
            playerData = GameObject.FindObjectOfType<PlayerCustomizationData>();
            string nickname = playerData.players[i].playerEntity.nickname;
            float score = playerData.players[i].playerEntity.score;
            instance.highscoresDemo.nicknameToSend = nickname;
            instance.highscoresDemo.scoreToSend = score;
            instance.highscoresDemo.Insert();
            Debug.Log("empikame2");
            Debug.Log(nickname + " - " + score);
        }
    }
}
