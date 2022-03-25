using TMPro;
using UnityEngine;

public class AftermatchScoreboard : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject afterMatchResults;
    [SerializeField] GameObject newHighScore;
    [Header("Texts")]
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] TMP_Text highScoreText;

    Scoreboard _scoreBoard;

    string _highScoreKey = "Highscore"; // the string used to identify the "Highscore" variable in PlayerPrefs

    // Start is called before the first frame update
    void Start()
    {
        _scoreBoard = FindObjectOfType<Scoreboard>();
    }

    public void SetHighScore()
    {
        int finalScore = _scoreBoard.GetScore();
        int highScore = PlayerPrefs.HasKey(_highScoreKey) ? PlayerPrefs.GetInt(_highScoreKey) : 0;

        if (finalScore > highScore)
        {
            PlayerPrefs.SetInt(_highScoreKey, finalScore);
            newHighScore.SetActive(true);
        }
    }

    public void UpdateFinalScoreboard()
    {
        afterMatchResults.SetActive(true);
        finalScoreText.text = _scoreBoard.GetScore().ToString();
        highScoreText.text = PlayerPrefs.GetInt(_highScoreKey).ToString();
    }
}
