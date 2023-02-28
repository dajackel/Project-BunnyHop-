using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour//, IUnityAdsInitializationListener
{
    public SceneTrackingScript SceneManager;
    [SerializeField] UIManager UI;
    [SerializeField] GameObject lLevelBound, rLevelBound, BLevelBound;
    public bool paused = false;
    private float timescaleAtLose = 0, currentLevelPos = 25;
    public GameObject[] levelSection;
    public GameObject[] powerUps;
    private GameObject lastLevelSpawned;
    private bool creatingLevel = false;
    private AudioSource backgroundMusic;
    private AudioSource lossSFX;

    private float maxFallDist = 26.0f;

    //Player reference to easily grab height
    [SerializeField] GameObject player;

    //Enemy prefab for random spawning
    [SerializeField] GameObject enemy;
    private bool canSpawnEnemy = false;

    private GameObject[] currentLevelSections = new GameObject[5];
    private int newestLevel = 0;

    //player highscore
    public static float HighScore, bestHeightThisRun;
    public static GAME_STATE gameState;

    public enum GAME_STATE
    {
        GAME_RUNNING,
        GAME_PAUSED,
        GAME_CONTINUE,
        GAME_OVER,
        MAIN_MENU,
        GAME_RESTART,
    }

    void Start()
    {
        SceneManager = GameObject.FindGameObjectWithTag("PreviousScene").GetComponent<SceneTrackingScript>();

        bestHeightThisRun = 0;
        backgroundMusic = Camera.main.GetComponents<AudioSource>()[0];
        lossSFX = Camera.main.GetComponents<AudioSource>()[1];
        HighScore = PlayerPrefs.GetFloat("HighScore", 0.0f);
        UI.updateHighScore(HighScore);
        BLevelBound.transform.position = new Vector2(BLevelBound.transform.position.x, -8.4f);
        setGameState(GAME_STATE.GAME_RUNNING);
        StartCoroutine(enemySpawnDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
            return;
        PlayerScript pScript = player.GetComponent<PlayerScript>();
        if (!UI.getIsFadeCompleted())
            pScript.enabled = false;
        else
            pScript.enabled = true;

        if (pScript.lose == true || BLevelBound.transform.position.y >= player.transform.position.y)
            setGameState(GAME_STATE.GAME_OVER);
        if (!creatingLevel)
            StartCoroutine(levelGenerator());
        float pHeight = player.transform.position.y/* + player.GetComponent<BoxCollider2D>().size.y / 2*/;
        lLevelBound.transform.position = new Vector3(lLevelBound.transform.position.x, pHeight, lLevelBound.transform.position.z);
        rLevelBound.transform.position = new Vector3(rLevelBound.transform.position.x, pHeight, rLevelBound.transform.position.z);

        BLevelBound.transform.position = new Vector2(BLevelBound.transform.position.x, Mathf.Clamp(BLevelBound.transform.position.y, player.transform.position.y - maxFallDist, BLevelBound.transform.position.y));

        if (pHeight > bestHeightThisRun)
            bestHeightThisRun = pHeight;
        if (pHeight > HighScore)
        {
            HighScore = pHeight;
            UI.updateHighScore(HighScore);
            PlayerPrefs.SetFloat("HighScore", HighScore);
        }
        UI.bestHeightThisRun = bestHeightThisRun;
    }
    private GameObject randomItemGen()
    {
        float extraLifeChance = 2f,
            invincibilityChance = 15;
        //item[0] = extra life, item[1] = invincibility
        float spawnVal = Random.Range(1, 100);
        if (spawnVal <= extraLifeChance)
        {
            //choose extra life
            return powerUps[0];
        }
        else if (spawnVal <= invincibilityChance)
        {
            //choose invincibility
            return powerUps[1];
        }
        return null;
    }
    IEnumerator levelGenerator()
    {
        float pHeight = player.GetComponent<PlayerScript>().getPlayerHeight();
        if (currentLevelPos >= pHeight + 100)
            yield return new WaitUntil(() => currentLevelPos <= pHeight + 100);
        float ENEMY_SPAWN_CHANCE = 25;
        GameObject spawnedSection = null;
        creatingLevel = true;
        currentLevelPos += 26;

        //spawn level section
        int lvlToSpawn = Random.Range(1, levelSection.Length);
        if (lastLevelSpawned == levelSection[lvlToSpawn])
            lvlToSpawn = (lvlToSpawn + 1) % levelSection.Length;
        spawnedSection = Instantiate(levelSection[lvlToSpawn], new Vector3(-1.25f, currentLevelPos, 0), Quaternion.identity);
        lastLevelSpawned = levelSection[lvlToSpawn];

        //spawn enemies
        float spawnVal = Random.Range(1, 100);
        if (canSpawnEnemy && spawnVal <= ENEMY_SPAWN_CHANCE)
        {
            //Enemy needs to spawn
            bool leftOrRightSide = (spawnVal % 2 == 0) ? true : false;
            GameObject curEnemy = Instantiate(enemy, new Vector3((leftOrRightSide) ? -14 : 13, Random.Range(pHeight, pHeight + 25), 0), (leftOrRightSide) ? Quaternion.identity : Quaternion.Euler(0, 180, 0));
        }

        //spawn items
        GameObject chosenItem = randomItemGen();
        if (chosenItem != null)
        {
            //find viable position within level section just spawned
            Transform[] objInLvl = spawnedSection.GetComponentsInChildren<Transform>();
            //select random platform
            int platform = Random.Range(2, objInLvl.Length);

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
            Destroy(currentLevelSections[newestLevel]);
        }
        currentLevelSections[newestLevel] = newLevelSection;
        newestLevel = (newestLevel + 1 == currentLevelSections.GetUpperBound(0) + 1) ? 0 : newestLevel + 1;
        yield return null;
    }

    IEnumerator enemySpawnDelay()
    {
        yield return new WaitForSecondsRealtime(10);
        canSpawnEnemy = true;
    }
    public void setGameState(GAME_STATE gs)
    {
        gameState = gs;
        switch (gameState)
        {
            case GAME_STATE.GAME_RUNNING:
                paused = false;
                backgroundMusic.volume = 0.3f;
                backgroundMusic.UnPause();
                Time.timeScale = 1;
                break;
            case GAME_STATE.GAME_PAUSED:
                backgroundMusic.volume = 0.15f;
                Time.timeScale = 0;
                paused = true;
                break;
            case GAME_STATE.GAME_OVER:
                paused = true;
                lossSFX.Play();
                backgroundMusic.volume = 0.15f;
                backgroundMusic.Pause();
                timescaleAtLose = Time.timeScale;
                UI.extraLifeCount = player.GetComponent<PlayerScript>().getExtraLifeCount();
                Time.timeScale = 0;
                PlayerPrefs.Save();
                UI.lossScreenTrigger();
                break;
            case GAME_STATE.GAME_CONTINUE:
                if (lossSFX.isPlaying)
                    lossSFX.Stop();
                backgroundMusic.volume = 0.3f;
                backgroundMusic.UnPause();
                player.transform.position = new Vector3(player.transform.position.x, BLevelBound.transform.position.y + 0.2f, player.transform.position.z);
                var pScript = player.GetComponent<PlayerScript>();
                pScript.lose = false;
                pScript.useExtraLife();
                UI.lossScreenTrigger();
                paused = false;
                setGameState(GAME_STATE.GAME_RUNNING);
                Time.timeScale = timescaleAtLose;
                break;
            case GAME_STATE.MAIN_MENU:
                lossSFX.mute = true;
                PlayerPrefs.Save();
                SceneManager.LoadScene("MainMenu");
                break;
            case GAME_STATE.GAME_RESTART:
                paused = false;
                bestHeightThisRun = 0.0f;
                SceneManager.LoadScene("MainGame");
                break;
        }
    }
    public GAME_STATE getGameState() { return gameState; }

}
