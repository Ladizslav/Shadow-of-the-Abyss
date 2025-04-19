using UnityEngine;
using TMPro;

public class CollectibleCounter : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();


    }
    private void Update()
    {
        UpdateCollectibleCount();
    }

    public void UpdateCollectibleCount()
    {
        textMeshPro.text = $"Collected: {CollectibleManager.Instance.collectedCollectibles}/{CollectibleManager.Instance.collectiblesNeeded}";
    }
}
