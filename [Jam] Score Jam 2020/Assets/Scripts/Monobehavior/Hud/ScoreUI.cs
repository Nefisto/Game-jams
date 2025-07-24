using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public IntReference score;

    private void Awake ()
    {
        score.Variable.AfterChangeValue += UpdateScore;
    }

    private void UpdateScore (int score)
    {
        scoreText.text = score.ToString().PadLeft(7, '0');
    }
}
