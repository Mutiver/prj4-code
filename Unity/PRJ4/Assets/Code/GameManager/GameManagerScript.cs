using TMPro;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] private int _startingHealth = 100;
    [SerializeField] private int _money = 300;
    [SerializeField] private int _currentWave = 1;
    [SerializeField] private int _difficulty = 3;

    public int enemyCount = 0;
    public NewWaveSpawner newWaveSpawner;
    public DatabaseManager db;

    //Canvas
    public GameObject gameOverScreen;
    public GameObject playButton;
    public GameObject shopBorder;
    public GameObject TowerMenu;
    public GameObject gameCompleted;
    public TextMeshProUGUI healthUI;
    public TextMeshProUGUI moneyUI;
    public TextMeshProUGUI currentWaveCounter;
    public GameObject upgradeMenu;

    public enum RoundStates
    {
        Idle,
        Spawning,
        InProgress
    }
    public RoundStates roundState = RoundStates.Idle;
    // Start is called before the first frame update
    void Start()
    {
        db = GameObject.Find("DatabaseManager").GetComponent<DatabaseManager>();
        if (GameDataScript.Instance != null)
        {
            _difficulty = GameDataScript.Instance.Difficulty;
            SetValuesOnDifficulty();
        }
        StartMethod();
    }

    private void StartMethod()
    {
        UpdateHealth();
        UpdateMoney();
        UpdateWave();
    }

    private void SetValuesOnDifficulty()
    {
        if (_difficulty == 1)
        {
            _health = 300;
            _startingHealth = 300;
            _money = 600;
        }
        if (_difficulty == 2)
        {
            _health = 200;
            _startingHealth = 200;
            _money = 450;
        }
        if (_difficulty == 3)
        {
            _health = 100;
            _startingHealth = 100;
            _money = 300;
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateMethod();
    }

    private void UpdateMethod()
    {
        if (enemyCount <= 0 && roundState == RoundStates.InProgress)
        {
            _currentWave++;
            roundState = RoundStates.Idle;
            UpdateWave();
        }
        if (_health <= 0)
        {
            gameOverScreen.SetActive(true);
            playButton.SetActive(false);
            TowerMenu.SetActive(false);
            upgradeMenu.SetActive(false);
        }

        if (newWaveSpawner.allWavesSpawned && enemyCount <= 0)
        {
            GameCompleted();
        }
        UpdateHealth();
        UpdateMoney();
    }

    /// <summary>
    /// Used to update the UI health
    /// </summary>
    private void UpdateHealth()
    {
        healthUI.text = _health.ToString();
    }
    /// <summary>
    /// Used to update the UI money
    /// </summary>
    private void UpdateMoney()
    {
        moneyUI.text = _money.ToString();
    }

    private void UpdateWave()
    {
        currentWaveCounter.text = $"Wave {_currentWave}";
    }

    /// <summary>
    /// When an enemy dies it HAS to call this function
    /// </summary>
    /// <param name="enemy">This.gameobject</param>
    public void EnemyDied(GameObject enemy)
    {
        enemyCount--;
        //Debug.Log(enemy.name);
        _money += enemy.GetComponent<MyEnemyScript>().goldValue;
    }
    /// <summary>
    /// When an enemy spawns it HAS to call this function
    /// </summary>
    public void EnemySpawned()
    {
        enemyCount++;
    }

    /// <summary>
    /// When an enemy gets to the base and attacks it will call this function
    /// </summary>
    /// <param name="damage">The amount of damage dealt</param>
    public void BaseHit(int damage)
    {
        int calculatedDamage = damage / 5; 
        _health -= calculatedDamage > 5 ? calculatedDamage : 5; //deals minimum 5 damage per monster
        enemyCount--;
        UpdateHealth();
    }
    /// <summary>
    /// When the base is healed this function is called
    /// </summary>
    /// <param name="repair">Amount of health healed</param>
    public void BaseRepair(int repair)
    {
        _health += repair;
        UpdateHealth();
    }

    public int GetMoney()
    {
        return _money;
    }

    public void MoneySpend(int money)
    {
        _money -= money;
    }

    public void MoneyGained(int money)
    {
        _money += money;
    }

    public void WaveSpawned()
    {
        roundState = RoundStates.InProgress;
    }

    public void GameCompleted()
    {
        gameCompleted.SetActive(true);
        playButton.SetActive(false);
        TowerMenu.SetActive(false);
        upgradeMenu.SetActive(false);
    }
    

    public void StartWave()
    {
        if (roundState == RoundStates.Idle)
        {
            roundState = RoundStates.Spawning;
            //Add function to enemy spawner here
            StartCoroutine(newWaveSpawner.SpawnWave());
        }
    }

    public void SetHiscore()
    {
        db.highScore = _currentWave;
        db.SetHighscore();
    }
}
