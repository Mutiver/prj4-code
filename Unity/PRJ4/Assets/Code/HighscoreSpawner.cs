
//public class HighscoreSpawner : MonoBehaviour
//{
//    public DatabaseManager DatabaseManager;
//	public GameObject highscorePrefab;
//	public RectTransform highscoreParent;
//	public TextMeshProUGUI textMeshProUGUI;
//	public void SpawnHighscore()
//	{

//		DatabaseManager.GetHighscore(highscoreList =>
//		{
//			Debug.Log("the highscorelist count is: " + highscoreList.Count);

//			// Loop through the highscore list and create a UI Text element for each entry
//			for (int i = 0; i < highscoreList.Count; i++)
//			{
//				Debug.Log("the highscorelist is: " + highscoreList[i]);
//				textMeshProUGUI.text += highscoreList[i];


//				// Set the text of the UI element to the current highscore data
//				//highscoreText.GetComponent<TextMeshProUGUI>().text = highscoreList[i];

//			}
//			highscoreList.Clear();
//		});
//	}
//}
