using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openUpgrade : MonoBehaviour
{
    public GameObject upgradeMenu;
    public TowerScript tower;

    private void Start()
    {
        upgradeMenu = GameObject.Find("GameManager").GetComponent<GameManagerScript>().upgradeMenu;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1)) upgradeMenu.SetActive(false);
    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            upgradeMenu.SetActive(true);
            upgradeMenu.GetComponent<UpgradeMenu>().tower = tower;
            tower.UpdateUIStats();
        }
    }
}
