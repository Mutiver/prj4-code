using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public Button playButton;
    public GameManagerScript gm;

    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(StartWave);
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (gm.roundState != GameManagerScript.RoundStates.Idle)
        {
            image.color = new Color32(128, 128, 128, 255);
        }
        else
        {
            image.color = new Color32(255, 255, 255, 255);
        }
    }

    //Cannot be Unit Tested
    private void StartWave()
    {
        gm.StartWave();
    }
}
