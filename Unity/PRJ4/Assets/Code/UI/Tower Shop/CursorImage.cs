using UnityEngine;

//CANNOT BE UNIT TESTED
public class CursorImage : MonoBehaviour
{
    public TowerMenuToggle _shop;

    public GameObject tower;
    public Transform rangeIndicator;
    public GameManagerScript gmScript;

    public GameObject[] antiTowerZones;
    public bool _noNoZone;

    CapsuleCollider2D cursorCollider;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        GetComponent<SpriteRenderer>().sprite = tower.GetComponent<SpriteRenderer>().sprite;

        cursorCollider = gameObject.AddComponent<CapsuleCollider2D>();
        var tempCollider = tower.GetComponent<TowerScript>().towerZone;
        cursorCollider.direction = tempCollider.direction;
        cursorCollider.size = tempCollider.size;
        cursorCollider.offset = tempCollider.offset;

        cursorCollider.isTrigger = true;

        Vector3 newScale = rangeIndicator.localScale * (tower.GetComponent<TowerScript>().range * 2);
        rangeIndicator.localScale = newScale;

        antiTowerZones = GameObject.FindGameObjectsWithTag("AntiTowerZone");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(cursorPosition.x, cursorPosition.y, 10);
        if (_noNoZone) { GetComponent<SpriteRenderer>().color = new Color32(255, 150, 150, 255); }
        else { GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255); }

        if (Input.GetMouseButtonDown(0))
        {
            if (!_noNoZone)
            {
                Debug.Log("Tower Created");
                var tempTower = Instantiate(tower, transform.position, transform.rotation);
                tempTower.GetComponent<TowerScript>().GameManager = gmScript;
                gmScript.MoneySpend(tower.GetComponent<TowerScript>().price);
                Destroy(gameObject);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        _shop.SetShop(TowerMenuToggle.ShopState.open);
        Cursor.visible = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("AntiTowerZone") || collision.CompareTag("TowerZone")) _noNoZone = true;
        //        Debug.Log("Trigger Stat");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger Exit");
        _noNoZone = false;
    }
}
