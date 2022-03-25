using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    PlayerCollisions _player;
    TMP_Text _scoreBoard;

    int _score;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerCollisions>();
        _scoreBoard = GetComponent<TMP_Text>();
        _scoreBoard.text = "No Score";
    }

    public void IncreaseScore(int amount)
    {
        if (!_player.IsAlive()) { return; }

        _score += amount;
        _scoreBoard.text = _score.ToString();
    }

    public int GetScore()
    {
        return _score;
    }
}
