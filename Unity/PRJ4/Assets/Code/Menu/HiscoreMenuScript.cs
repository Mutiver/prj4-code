using TMPro;
using UnityEngine;


public class HiscoreMenuScript : MonoBehaviour
{
    public DatabaseManager DatabaseManager;
    [SerializeField] private TextMeshProUGUI myTextField;
    private string highscoreString;
    private int count = 30;

    void Start()
    {
        DatabaseManager = GameObject.Find("DatabaseManager").GetComponent<DatabaseManager>();
    }
    public void UpdateText()
    {
        DatabaseManager.GetHighscore(highscoreString =>
        {
            myTextField.text = highscoreString;
            myTextField.ForceMeshUpdate(true);
        });
    }

    private void FixedUpdate()
    {
        if (count >= 60)
        {
            myTextField.ForceMeshUpdate(true);
            count = 0;
        }
        else
        {
            count++;
        }
    }
}