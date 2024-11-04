using System.Collections;
using UnityEngine;

public class NewWaveSpawner : MonoBehaviour
{
    // Enum to represent the different states of the spawner
    public enum RoundStates { Spawning, Idle, InProgress };

    // Class to define a wave's properties
    [System.Serializable]
    public class Wave
    {
        [Header("Wave settings")] //Header for the UI in Unity
        public string name; // Name of the wave
        public GameObject enemy;  // Type of enemy to spawn
        public int count;   // The number of enemies to spawn
        public float rate;  // The rate at which to spawn enemies 
    }

    [Header("General Wave Settings")] //Header for the UI in Unity
    public float timeBetweenWaves = 2f; // The time between waves
    public float waveCountdown = 1; // The countdown until the next wave

    public Wave[] waves; // Array of all waves
    public int nextWave = 0; // Index of the next wave
    public static int enemiesAlive = 0; // The number of enemies currently alive
    public bool allWavesSpawned = false;
    private float searchForEnemyCountdown = 1f; // The time between checking for enemies
    private RoundStates state = RoundStates.InProgress; // The current state of the spawner
    public GameManagerScript gm;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private bool EnemyIsAlive() // Not used anymore
    {
        // Count down until the next search for enemies
        searchForEnemyCountdown -= Time.deltaTime;

        if (searchForEnemyCountdown <= 0f)
        {
            // Reset the countdown and check for enemies
            searchForEnemyCountdown = 1f;

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                // No enemies found
                return false;
            }
        }

        // At least one enemy is alive so return true
        return true;
    }

    public IEnumerator SpawnWave()
    {
        // if (nextWave >= waves.Length) // nextWave out of bounds. Yield break to stop
        // {
        //     gm.GameCompleted();
        //     yield break;
        // }

        Debug.Log("Spawning wave");
        Wave wave = waves[nextWave];
        // Spawn the wave and log
        state = RoundStates.Spawning;

        for (int i = 0; i < wave.count; i++) // number of enemies
        {
            // Spawn an enemy
            SpawnEnemy(wave.enemy); 

            // Wait abit for the next enemy to be spawned in the wave
            yield return new WaitForSeconds(1f / wave.rate);
        }

        // All enemies have been spawned
        WaveCompleted();
        state = RoundStates.Idle;

        yield break; // not necesary but makes it clear what the intend is
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        // Spawn an enemy and increment the enemy count and log
        Debug.Log("Spawning enemy: " + enemyPrefab.name);
        var enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        enemy.GetComponent<MyEnemyScript>().gameManagerScript = gm;
        gm.EnemySpawned();
        enemiesAlive++;
    }

    private void WaveCompleted()
    {
        // The wave has been completed therefore log
        Debug.Log("Wave completed!");

        // Reset the countdown and increment the wave index
        state = RoundStates.Idle;
        
        if (nextWave >= waves.Length - 1)
        {
            // All waves completed
            Debug.Log("All waves completed.");
            allWavesSpawned = true;
        }
        else
        {
            // Else increments to next wave
            nextWave++;
            gm.WaveSpawned();
        }

    }

    public void StartCountDownForWave() // Not used anymore
    {
        waveCountdown = timeBetweenWaves;
    }
}
