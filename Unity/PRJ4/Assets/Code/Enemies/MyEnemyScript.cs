using System;
using UnityEngine;

public class MyEnemyScript : MonoBehaviour
{

    public float speed = 2f; //speed
    public float maxHealth = 10;
    public float health = 10;
    public int goldValue = 30;
    [SerializeField] private HealthBarScript _healthBar;
    public GameObject deathEffect;
    public GameManagerScript gameManagerScript;
    private Transform target;
    private int waypointIndex = 0;

    private bool hitBase = false;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        _healthBar.UpdateHealthBar(maxHealth, health);
        target = MyWaypointScript.points[0];
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        _healthBar.UpdateHealthBar(maxHealth, health);
        if (health <= 0)
        {
            EnemyDies();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= MyWaypointScript.points.Length - 1) // if last waypoint is reached
        {
            hitBase = true;
            Debug.Log("Waypoint index out of range. Destroying object and base hit.");
            Destroy(gameObject); // destroy monster
            gameManagerScript.BaseHit((int)(health));
            return;
        }
        
        waypointIndex++; // else increments waypoint counter
        target = MyWaypointScript.points[waypointIndex]; // set target waypoint
    }
    void EnemyDies()
    {
        Destroy(gameObject); // Destroy game object on death
    }

    private void OnDestroy()
    {
        if (hitBase) return;
        GameObject deathEffectIns = (GameObject)Instantiate(deathEffect, transform.position, transform.rotation); // spawn deathparticle
        Destroy(deathEffectIns, 3f); // destroy death particle object

        NewWaveSpawner.enemiesAlive--; // decrement counter
        gameManagerScript.EnemyDied(this.gameObject); // gain money from kill
    }
}
