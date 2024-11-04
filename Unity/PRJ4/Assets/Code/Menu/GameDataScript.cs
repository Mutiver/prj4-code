using UnityEngine;

public class GameDataScript : MonoBehaviour
{
    public static GameDataScript Instance { get; private set; }

    [SerializeField] private int _difficulty;

    public int Difficulty
    {
        get => _difficulty;
        set => _difficulty = value;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Difficulty = 0;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}