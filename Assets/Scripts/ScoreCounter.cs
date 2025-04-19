using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        UpdateScoreCount(); 
    }

    private void Update()
    {
        UpdateScoreCount();
    }

    public void UpdateScoreCount()
    {
        if (PlayerScore.Instance != null && textMeshPro != null)
        {
            textMeshPro.text = $"Areas Cleared: {PlayerScore.Instance.score}/{PlayerScore.Instance.scoreToWin}";
        }
    }
}
