using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    private int score;
    private int highScore;
    public Player player;
    public TMP_Text ScoreText;
    public GameObject ScoreBoard;
    public TMP_Text SBScore;
    public TMP_Text HighScore;
    public GameObject PlayButton;
    public GameObject gameOver;
    public GameObject StartText;
    public GameObject Intro;
    public GameObject ScoreDisplay;
    public GameObject ObstaclesSpawner;
    public GameObject OceanFloor;
    public GameObject PausePanel;
    public GameObject PauseButton;
    public GameObject ResumeButtom;
    public ParticleSystem DarkParticle;
    public ParticleSystem DustParticle;
    public GameObject BotObstacle;
    public bool scored = false;
    public bool newHigh = false;
    public AudioSource Background;

    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _iOSAdUnitId = "Banner_iOS";
    string _adUnitId = null;




    private void Awake()
    {
        Pause();
        Application.targetFrameRate = 60;
        LoadPlayerInfo();
        DarkParticle.gameObject.SetActive(false);
        
    }

    private void Start()
    {
        Vector2 BottomLeftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector2 TopRightEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        DustParticle.gameObject.transform.position = new Vector2(0, TopRightEdge.y + 1);
        DarkParticle.gameObject.transform.position = new Vector2(0, BottomLeftEdge.y - 0.2f);
        BotObstacle.transform.position = new Vector2(0, BottomLeftEdge.y -0.2f);
        gameOver.SetActive(false);
        ScoreBoard.SetActive(false);
        PlayButton.SetActive(true);
        Intro.SetActive(true);
        ScoreDisplay.SetActive(false);
        PausePanel.SetActive(false);
        PauseButton.SetActive(false);
        ResumeButtom.SetActive(false);

        Background = gameObject.GetComponent<AudioSource>();
        Background.Play();
        //OceanFloor.GetComponent<Renderer>().material.SetFloat("Vector1_d01672ad36644d51bf201747a6bb5681",0);

        Pause();


#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
        Advertisement.Banner.SetPosition(_bannerPosition);
        Advertisement.Banner.Load(_adUnitId);
    }

    void LoadPlayerInfo()
    {
        string fileName = Application.persistentDataPath + "PlayerInfo.xml";
        //string fileName = "C:/Unity Projects/Beyond/PlayerInfo.xml";

        try
        {
            XElement root = XElement.Load(fileName);
            highScore = int.Parse(root.Value);
        }
        catch (System.Exception e)
        {
            XmlWriter writer = XmlWriter.Create(fileName);
            writer.WriteElementString("HighScore", "0");
            writer.Flush();
        }

    }

    void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_adUnitId, options);
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }

    public void GameOver()
    {
        Pause();
        player.animator.speed = -1;
        if (score > highScore)
        {
            highScore = score;
            saveHighScore(highScore);
        }
        gameOver.SetActive(true);
        PlayButton.SetActive(true);
        ScoreBoard.SetActive(true);
        ScoreDisplay.SetActive(false);
        PausePanel.SetActive(false);
        SBScore.text = score.ToString();
        HighScore.text = highScore.ToString();

        //Advertisement.Banner.Hide();
    }

    public void PauseButtonPressed()
    {
        Pause();
        player.animator.speed = -1;
        PauseButton.SetActive(false);
        ResumeButtom.SetActive(true);
    }

    public void resumeButtomPressed()
    {
        player.enabled = true;
        player.animator.speed = 1;
        PauseButton.SetActive(true);
        ResumeButtom.SetActive(false);

        ObstaclesSpawner.SetActive(true);

        Obstacle[] obs = FindObjectsOfType<Obstacle>();
        JellyFish[] scores = FindObjectsOfType<JellyFish>();
        for (int i = 0; i < obs.Length; i++)
            obs[i].enabled = true;
        for (int i = 0; i < scores.Length; i++)
            scores[i].enabled = true;
    }

    void saveHighScore(int newHigh)
    {
        string fileName = Application.persistentDataPath + "PlayerInfo.xml";
        //string fileName = "C:/Unity Projects/Beyond/PlayerInfo.xml";
        try
        {
            XElement root = XElement.Load(fileName);
            root.Value = highScore.ToString();
            root.Save(fileName);
        }
        catch (System.Exception e)
        {
            XmlWriter writer = XmlWriter.Create(fileName);
            writer.WriteElementString("HighScore", highScore.ToString());
            writer.Flush();
        }
    }

    private void Update()
    {
        Message.MessageSent4?.Invoke(highScore.ToString());
    }

    public void Play()
    {
        PausePanel.SetActive(true);
        PauseButton.SetActive(true);
        PlayButton.SetActive(false);
        gameOver.SetActive(false);
        Intro.SetActive(false);
        ScoreBoard.SetActive(false);
        ScoreDisplay.SetActive(true);
        DarkParticle.gameObject.SetActive(true);
        ObstaclesSpawner.SetActive(true);
        score = 0;
        ScoreText.text = score.ToString();

        //Time.timeScale = 1;
        player.enabled = true;
        player.gameObject.transform.position = new Vector2(0, -2);

        Obstacle[] obs = FindObjectsOfType<Obstacle>();
        JellyFish[] scores = FindObjectsOfType<JellyFish>();
        for (int i = 0; i < obs.Length; i++)
            Destroy(obs[i].gameObject);
        for (int i = 0; i < scores.Length; i++)
            Destroy(scores[i].gameObject);

        Advertisement.Banner.Show(_adUnitId);
    }

    public void IncreaseScore()
    {
        if (scored)
            return;
        scored = true;
        Debug.Log(score.ToString());
        score++;
        ScoreText.text = score.ToString();
    }

    public void Pause()
    {
        player.enabled = false;
        player.rb.velocity = new Vector2(0,0);
        //Time.timeScale = 0;

        ObstaclesSpawner.SetActive(false);

        Obstacle[] obs = FindObjectsOfType<Obstacle>();
        JellyFish[] scores = FindObjectsOfType<JellyFish>();
        for (int i = 0; i < obs.Length; i++)
            obs[i].enabled = false;
        for (int i = 0; i < scores.Length; i++)
            scores[i].enabled = false;
    }



}
