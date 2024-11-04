using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenuImage : MonoBehaviour
{

    public TowerMenuToggle toggle;

    public GameObject gm;
    public GameManagerScript gmScript;

    public GameObject tower;
    public TowerScript towerScript;

    public Button btn;
    public Image imageComp;

    public GameObject cursorImage;

    public TextMeshProUGUI priceTag;

    public GameObject cursor;

    public bool Affordable;
    //public Texture2D towerImage;
    //public CursorMode cursorMode = CursorMode.Auto;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(btnClicked);

        StartMethod();
    }

    private void StartMethod()
    {
        gmScript = gm.GetComponent<GameManagerScript>();
        towerScript = tower.GetComponent<TowerScript>();
        priceTag.text = towerScript.price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAffordableCost();
    }

    private void UpdateAffordableCost()
    {
        if (gmScript.GetMoney() >= towerScript.price)
        {
            Affordable = true;
            imageComp.color = new Color32(255, 255, 255, 255);
            priceTag.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            Affordable = false;
            imageComp.color = new Color32(255, 150, 150, 255);
            priceTag.color = new Color32(255, 150, 150, 255);
        }
    }

    void btnClicked()
    {
        Debug.Log("Btn Clicked");
        if (!Affordable)
        {
            Debug.Log("Insufficient Funds");
            return;
        }
        cursor = Instantiate(cursorImage);
        cursor.GetComponent<CursorImage>().tower = tower;
        cursor.GetComponent<CursorImage>().gmScript = gmScript;
        cursor.GetComponent<CursorImage>()._shop = toggle;
        toggle.SetShop(TowerMenuToggle.ShopState.closed);
    }
}
