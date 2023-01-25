using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour//, IUnityAdsInitializationListener
{
    const string androidGameId = "", iosGameId = "",
       androidBannerUnit = "Banner_Android", iosBannerUnit = "Banner_iOS", FullScreenAd = "Pre-respawn_FS_ad";
    [SerializeField] Button _loadBannerButton, _showBannerButton, _hideBannerButton;
    //[SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
    //bool _testMode = true;
    //private string adUnitId;
    public bool paused = false;
    private float timescaleAtLose = 0, currentLevelPos = 25;
    public GameObject[] levelSection;
    public GameObject[] powerUps;
    private GameObject lastLevelSpawned;
    [SerializeField] UIManager UI;
    [SerializeField] GameObject lLevelBound, rLevelBound, BLevelBound;
    private bool creatingLevel = false;
    float backgroundColorChangeVal = 0;
    int nextColorChange = 500;

    private float maxFallDist = 26.0f;

    //Player reference to easily grab height
    [SerializeField] PlayerScript player;

    //Enemy prefab for random spawning
    [SerializeField] GameObject enemy;

    private GameObject[] currentLevelSections = new GameObject[5];
    private int newestLevel = 0;

    //player highscore
    public static float highScore, bestHeightThisRun = 0;
    public static GAME_STATE gameState;

    public enum GAME_STATE
    {
        GAME_RUNNING,
        GAME_PAUSED,
        GAME_CONTINUE,
        GAME_OVER,
        MAIN_MENU,
        GAME_EXIT,
        GAME_START,
        GAME_RESTART,
    }
    private void Awake()
    {
        //adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
        //   ? iosBannerUnit
        //   : androidBannerUnit;
    }

    //    #region BANNERADS
    //    public void InitializeAds()
    //    {
    //        Advertisement.Initialize(_gameId, _testMode, this);
    //    }
    //    public void OnInitializationComplete()
    //    {
    //        Debug.Log("Unity Ads initialization complete.");
    //    }
    //    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    //    {
    //        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    //    }
    //    // Implement code to execute when the loadCallback event triggers:
    //    void OnBannerLoaded()
    //    {
    //        Debug.Log("Banner loaded");

    //        // Configure the Show Banner button to call the ShowBannerAd() method when clicked:
    //        _showBannerButton.onClick.AddListener(ShowBannerAd);
    //        // Configure the Hide Banner button to call the HideBannerAd() method when clicked:
    //        _hideBannerButton.onClick.AddListener(HideBannerAd);

    //        // Enable both buttons:
    //        _showBannerButton.interactable = true;
    //        _hideBannerButton.interactable = true;
    //    }

    //    // Implement code to execute when the load errorCallback event triggers:
    //    void OnBannerError(string message)
    //    {
    //        Debug.Log($"Banner Error: {message}");
    //        // Optionally execute additional code, such as attempting to load another ad.
    //    }

    //    // Implement a method to call when the Show Banner button is clicked:
    //    void ShowBannerAd()
    //    {
    //        // Set up options to notify the SDK of show events:
    //        BannerOptions options = new BannerOptions
    //        {
    //            clickCallback = OnBannerClicked,
    //            hideCallback = OnBannerHidden,
    //            showCallback = OnBannerShown
    //        };

    //        // Show the loaded Banner Ad Unit:
    //        Advertisement.Banner.Show(bannerAd, options);
    //    }

    //    // Implement a method to call when the Hide Banner button is clicked:
    //    void HideBannerAd()
    //    {
    //        // Hide the banner:
    //        Advertisement.Banner.Hide();
    //    }

    //    void OnBannerClicked() { }
    //    void OnBannerShown() { }
    //    void OnBannerHidden() { }

    //    void OnDestroy()
    //    {
    //        // Clean up the listeners:
    //        _loadBannerButton.onClick.RemoveAllListeners();
    //        _showBannerButton.onClick.RemoveAllListeners();
    //        _hideBannerButton.onClick.RemoveAllListeners();
    //    }
    //    public void LoadBanner()
    //    {
    //        // Set up options to notify the SDK of load events:
    //        BannerLoadOptions options = new BannerLoadOptions
    //        {
    //            loadCallback = OnBannerLoaded,
    //            errorCallback = OnBannerError
    //        };

    //        // Load the Ad Unit with banner content:
    //        Advertisement.Banner.Load(bannerAd, options);
    //    }
    ////#endregion

    void Start()
    {
        //_showBannerButton.interactable = false;
        //_hideBannerButton.interactable=false;

        //Advertisement.Banner.SetPosition(_bannerPosition);

        //_loadBannerButton.onClick.AddListener(LoadBanner);
        //_loadBannerButton.interactable = true;
        gameState = GAME_STATE.GAME_RUNNING;
        Time.timeScale = 1.0f;
        highScore = PlayerPrefs.GetFloat("highScore", 0.0f);
        UI.updateHighScore(highScore);
        BLevelBound.transform.position = new Vector2(BLevelBound.transform.position.x, player.transform.position.y - maxFallDist);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
            return;
        if (player.lose == true || BLevelBound.transform.position.y >= player.transform.position.y)
            setGameState(GAME_STATE.GAME_OVER);
        if (!creatingLevel)
            StartCoroutine(levelGenerator());
        //top of player's head position
        float pHeight = player.getPlayerHeight();
        lLevelBound.transform.position = new Vector3(lLevelBound.transform.position.x, pHeight, lLevelBound.transform.position.z);
        rLevelBound.transform.position = new Vector3(rLevelBound.transform.position.x, pHeight, rLevelBound.transform.position.z);

        BLevelBound.transform.position = new Vector2(BLevelBound.transform.position.x, Mathf.Clamp(BLevelBound.transform.position.y, player.transform.position.y - maxFallDist, BLevelBound.transform.position.y));

        if (pHeight > bestHeightThisRun)
            bestHeightThisRun = pHeight;
        if (pHeight > highScore)
        {
            highScore = pHeight;
            UI.updateHighScore(highScore);
            PlayerPrefs.SetFloat("highScore", highScore);
        }
        UI.pCurrHeight = pHeight;
        if (currentLevelPos >= nextColorChange)
        {
            nextColorChange += nextColorChange;
            backgroundColorChangeVal += 0.01f;
            print("Color change");
        }
    }

    IEnumerator levelGenerator()
    {
        if (currentLevelPos >= player.getPlayerHeight() + 100)
            yield return new WaitUntil(() => currentLevelPos <= player.getPlayerHeight() + 100);
        float ENEMY_SPAWN_CHANCE = 25;
        float extraLifeChance = 5f,
            invincibilityChance = 20f;
        GameObject spawnedSection = null;
        GameObject chosenItem = null;
        creatingLevel = true;
        currentLevelPos += 25;
        //spawn level section
        int lvlToSpawn = Random.Range(1, levelSection.Length);
        if (lastLevelSpawned == levelSection[lvlToSpawn])
            lvlToSpawn = (lvlToSpawn + 1) % levelSection.Length;
        spawnedSection = Instantiate(levelSection[lvlToSpawn], new Vector3(-1.25f, currentLevelPos, 0), Quaternion.identity);
        lastLevelSpawned = levelSection[lvlToSpawn];

        //color background by height
        spawnedSection.GetComponentsInChildren<SpriteRenderer>()[0].color = Color.Lerp(spawnedSection.GetComponentsInChildren<SpriteRenderer>()[0].color, Color.black, backgroundColorChangeVal);

        //spawn enemies
        float spawnVal = Random.Range(1, 100);
        if (spawnVal <= ENEMY_SPAWN_CHANCE)
        {
            //Enemy needs to spawn
            bool leftOrRightSide = (spawnVal % 2 == 0) ? true : false;
            GameObject curEnemy = Instantiate(enemy, new Vector3((leftOrRightSide) ? -14 : 13, Random.Range(player.getPlayerHeight(), player.getPlayerHeight() + 25), 0), (leftOrRightSide) ? Quaternion.identity : Quaternion.Euler(0, 180, 0));
            Rigidbody2D eRigidBody = curEnemy.GetComponent<Rigidbody2D>();
            if (leftOrRightSide)
                curEnemy.GetComponent<Rigidbody2D>().velocity = new Vector2(enemy.GetComponent<enemyScript>().speed, eRigidBody.velocity.y);
            else
                curEnemy.GetComponent<Rigidbody2D>().velocity = new Vector2(-enemy.GetComponent<enemyScript>().speed, eRigidBody.velocity.y);
        }

        //spawn items
        //item[0] = extra life, item[1] = invincibility
        spawnVal = Random.Range(1, 100);
        if (spawnVal <= extraLifeChance)
        {
            //choose extra life
            chosenItem = powerUps[0];
        }
        else if (spawnVal <= invincibilityChance)
        {
            //choose invincibility
            chosenItem = powerUps[1];
        }
        if (chosenItem != null)
        {
            //find viable position within level section just spawned
            Transform[] objInLvl = spawnedSection.GetComponentsInChildren<Transform>();
            //select random platform
            int platform = Random.Range(1, objInLvl.Length);

            //instantiate item slightly above chosen platform
            Vector3 pos = new Vector3(objInLvl[platform].position.x, objInLvl[platform].position.y + 2, objInLvl[platform].position.z);
            GameObject spawnedItem = Instantiate(chosenItem, pos, Quaternion.identity);
            spawnedItem.transform.parent = objInLvl[platform].transform.parent;
        }
        yield return new WaitForSeconds(4f);

        StartCoroutine(CleanUp(spawnedSection));
        creatingLevel = false;
    }
    IEnumerator CleanUp(GameObject newLevelSection)
    {

        if (currentLevelSections[newestLevel] != null)
        {
            yield return new WaitUntil(() => currentLevelSections[newestLevel].transform.position.y <= player.transform.position.y - 45);
            //yield return new WaitForSeconds(3);
            Destroy(currentLevelSections[newestLevel]);
        }
        currentLevelSections[newestLevel] = newLevelSection;
        newestLevel = (newestLevel + 1 == currentLevelSections.GetUpperBound(0) + 1) ? 0 : newestLevel + 1;
        yield return null;
    }
    public void setGameState(GAME_STATE gs)
    {
        gameState = gs;
        switch (gameState)
        {
            case GAME_STATE.GAME_RUNNING:
                Time.timeScale = 1;
                break;
            case GAME_STATE.GAME_PAUSED:
                Time.timeScale = 0;
                break;
            case GAME_STATE.GAME_OVER:
                timescaleAtLose = Time.timeScale;
                UI.extraLifeCount = player.getExtraLifeCount();
                Time.timeScale = 0;
                PlayerPrefs.Save();
                UI.lossScreenTrigger();
                break;
            case GAME_STATE.GAME_CONTINUE:
                player.transform.position = new Vector3(player.transform.position.x, BLevelBound.transform.position.y + 0.2f, player.transform.position.z);
                player.lose = false;
                player.useExtraLife();
                UI.lossScreenTrigger();
                setGameState(GAME_STATE.GAME_RUNNING);
                Time.timeScale = timescaleAtLose;
                break;
            case GAME_STATE.MAIN_MENU:
                Time.timeScale = 1;
                PlayerPrefs.Save();
                SceneManager.LoadScene("MainMenu");
                break;
            case GAME_STATE.GAME_EXIT:
                QuitGame();
                break;
            case GAME_STATE.GAME_RESTART:
                bestHeightThisRun = 0.0f;
                Time.timeScale = 1.0f;
                SceneManager.LoadScene("MainGame");
                break;
        }
    }
    public GAME_STATE getGameState() { return gameState; }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
