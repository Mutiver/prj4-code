using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class TowerScript : MonoBehaviour
{
    public int price;

    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();
    [SerializeField] private GameObject UpgradeUI;
    [SerializeField] private Button UpgradeButton;
    [SerializeField] private int baseUpgradeCost = 100;
    public float shootDelay = 1;
    public float delayCounter = 0;
    public GameObject projectile;
    public UpgradeMenu upgradeMenu;
    
    public GameObject enemy;
    public float range;
    private float closestDistance = Mathf.Infinity;
    public GameObject closestEnemy;
    public GameManagerScript GameManager;
    //bullet variable
    public float speed;
    public int bulletDamage;

    public float dirX;
    public float dirY;

    private int level = 0;
    private int previousUpgradeCost;
    private float basedmg;
    private float basespeed;
    private bool UIOpened = false; 
    public CapsuleCollider2D towerZone;

    public Button upgradeTowerAD;
    public Button upgradeTowerSpeed;
    public Button upgradeTowerRange;
    

    void Start()
    {
        baseUpgradeCost = price / 3;
        Physics2D.queriesHitTriggers = true;
        delayCounter = shootDelay;
        CircleCollider2D towerRange = GetComponent<CircleCollider2D>();
        towerRange.radius = range;
        basedmg = bulletDamage;
        //basespeed = speed;
        upgradeMenu = GameManager.upgradeMenu.GetComponent<UpgradeMenu>();
        upgradeTowerAD = upgradeMenu.upgradeAD;
        upgradeTowerSpeed = upgradeMenu.upgradeSpeed;
        upgradeTowerRange = upgradeMenu.upgradeRange;
        upgradeTowerAD.onClick.AddListener(UpgradeAD);
        upgradeTowerSpeed.onClick.AddListener(UpgradeSpeed);
        upgradeTowerRange.onClick.AddListener(UpgradeRange);
        
    }

    void Update()
    {
        if (delayCounter < shootDelay) delayCounter += Time.deltaTime;
        //Debug.Log(enemyList);
        if (enemyList.Count != 0)
        {
            enemyList.RemoveAll(item => item == null);

            closestEnemy = null;
            closestDistance = Mathf.Infinity;
            foreach (GameObject go in enemyList)
            {

                float distance = Vector2.Distance(go.transform.position, transform.position);
                
                if (closestDistance >= distance)
                {
                    closestDistance = distance;
                    closestEnemy = go;

                }
            }
            
            if (delayCounter >= 1 * shootDelay)
            {
                dirX = closestEnemy.transform.position.x;
                dirY = closestEnemy.transform.position.y;
                var angle = AngleBetweenPoints(transform.position, new Vector2(dirX, dirY));
                GameObject tempBullet = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle + 90));
                tempBullet.GetComponent<ProjetileScript>().speed = speed;
                tempBullet.GetComponent<ProjetileScript>().bulletDamage = bulletDamage;
                delayCounter = 0;

            }
        }
    }
    
    float AngleBetweenPoints(Vector2 tower, Vector2 target)
    {
        return Mathf.Atan2(tower.y - target.y, tower.x - target.x) * Mathf.Rad2Deg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

            enemyList.Add(collision.gameObject);
        }
    }
    
    public void UpgradeAD()
    {
        if (CalculateCost() > GameManager.GetMoney()) return;
        if (upgradeMenu.tower != this) return;
        
        GameManager.MoneySpend(CalculateCost());

        level++;

        int newDmgValue = (int)Math.Ceiling(bulletDamage * 1.2f);
        //minimum +2 dmg
        bulletDamage = newDmgValue < 3 ? newDmgValue : bulletDamage + 3;
        Debug.Log("Upgraded Attack Damage");
        UpdateUIStats();
    }
    
    public void UpgradeSpeed()
    {
        if (CalculateCost() > GameManager.GetMoney()) return;
        if (upgradeMenu.tower != this) return;
        
        GameManager.MoneySpend(CalculateCost());

        level++;
        
        shootDelay *= 0.82f;
        
        Debug.Log("Upgraded Attack speed");
        UpdateUIStats();
    }
    
    public void UpgradeRange()
    {
        if (CalculateCost() > GameManager.GetMoney()) return;
        if (upgradeMenu.tower != this) return;
        
        GameManager.MoneySpend(CalculateCost());

        level++;

        range += 3;
        CircleCollider2D towerRange = GetComponent<CircleCollider2D>();
        towerRange.radius = range;
        
        Debug.Log("Upgraded Attack speed");
        UpdateUIStats();
    }
    
    
    private int CalculateCost()
    {
        int costCalculated = Mathf.CeilToInt(baseUpgradeCost + (baseUpgradeCost*Mathf.Pow(level,1.5f)*0.4f));
        return (int)Math.Ceiling(costCalculated / 10.0) * 10;
    }
    
    public void CloseUpgradeUI()
    {
        UpgradeUI.SetActive(false);
    }

    public void UpdateUIStats()
    {
        upgradeMenu.AD.text = $"AD: {bulletDamage}";
        float speedInSecs = 1 / shootDelay;
        upgradeMenu.Speed.text = $"Speed: {speedInSecs.ToString("F2")}";
        upgradeMenu.Range.text = $"Range: {range}";
        upgradeMenu.Name.text = this.name.Replace("(Clone)", "");
        upgradeMenu.Level.text = $"Level: {level}";
        upgradeMenu.Cost.text = CalculateCost().ToString();
    }

     private void OnTriggerExit2D(Collider2D collision)
    {
        enemyList.Remove(collision.gameObject);
    }
}
