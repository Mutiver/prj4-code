using UnityEngine;
using UnityEngine.UI;

public class TowerMenuToggle : MonoBehaviour
{
    public enum ShopState
    {
        open, closed
    };

    private ShopState _shopState = ShopState.closed;

    public GameObject shopMenu;
    public GameObject noShopMenu;

    public Button cancelShop;
    public Button openShop;
    // Start is called before the first frame update
    void Start()
    {
        cancelShop.onClick.AddListener(ToggleShop);
        openShop.onClick.AddListener(ToggleShop);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleShop()
    {
        switch (_shopState)
        {
            case ShopState.open:
                noShopMenu.SetActive(true);
                shopMenu.SetActive(false);
                _shopState = ShopState.closed; break;
            case ShopState.closed:
                noShopMenu.SetActive(false);
                shopMenu.SetActive(true);
                _shopState = ShopState.open; break;
        }
    }

    public void SetShop(ShopState shopState)
    {
        if (shopState == _shopState)
        {
            Debug.Log("Shop is already in this state");
            return;
        }
        ToggleShop();
    }
}
