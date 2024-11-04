using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTowerMenu : MonoBehaviour
{
    public TowerMenuToggle toggle;
    public GameObject gm;
    public GameObject menuBackground;
    public GameObject towerImage;
    public List<GameObject> towerList = new List<GameObject>();
    public List<GameObject> towerImageList = new List<GameObject>();
    public int rowAmount = 2;
    public int currentRow;
    public int currentColumn;
    public float verticalMargin;
    public float horizontalMargin;
    // Start is called before the first frame update
    void Start()
    {
        StartMethod();
    }

    private void StartMethod()
    {
        currentRow = 0;
        currentColumn = 0;
        foreach (GameObject tower in towerList)
        {
            if (currentRow >= rowAmount)
            {
                currentRow = 0;
                currentColumn++;
            }
            currentRow++;
            var TowerImage = Instantiate(towerImage, transform);
            var towerTransform = TowerImage.GetComponent<RectTransform>();
            Vector2 newAnchor = new Vector2(horizontalMargin * currentColumn + horizontalMargin / 2, verticalMargin * currentRow - verticalMargin / 2);
            towerTransform.anchorMin = newAnchor;
            towerTransform.anchorMax = newAnchor;
            TowerImage.GetComponent<Image>().sprite = tower.GetComponent<SpriteRenderer>().sprite;
            TowerImage.GetComponent<TowerMenuImage>().tower = tower;
            TowerImage.GetComponent<TowerMenuImage>().gm = gm;
            TowerImage.GetComponent<TowerMenuImage>().toggle = toggle;
            towerImageList.Add(TowerImage);
        }
    }
}
