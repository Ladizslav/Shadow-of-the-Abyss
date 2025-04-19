using UnityEngine;
using UnityEngine.UI;
using TMPro; // P�id�no pro TextMeshPro
using Cinemachine;

public class CameraSensitivityController : MonoBehaviour
{
    [Header("Camera Reference")]
    public CinemachineFreeLook freeLookCamera;

    [Header("UI References")]
    public Slider sensitivitySlider;
    public TMP_Text sensitivityText; // Zm�n�no na TextMeshPro

    [Header("Settings")]
    [Range(0.1f, 10f)] public float minSensitivity = 0.5f;
    [Range(0.1f, 10f)] public float maxSensitivity = 5f;
    [Range(0.1f, 10f)] public float defaultSensitivity = 2f;

    private float _currentSensitivity;
    private const string SENSITIVITY_KEY = "CameraSensitivity";

    private void Start()
    {
        // Na�ten� ulo�en� hodnoty
        _currentSensitivity = PlayerPrefs.GetFloat(SENSITIVITY_KEY, defaultSensitivity);

        // Inicializace UI
        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = minSensitivity;
            sensitivitySlider.maxValue = maxSensitivity;
            sensitivitySlider.value = _currentSensitivity;
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }

        UpdateCameraSensitivity();
        UpdateSensitivityText();
    }

    public void SetSensitivity(float newValue)
    {
        _currentSensitivity = Mathf.Clamp(newValue, minSensitivity, maxSensitivity);
        UpdateCameraSensitivity();
        UpdateSensitivityText();

        PlayerPrefs.SetFloat(SENSITIVITY_KEY, _currentSensitivity);
        PlayerPrefs.Save();
    }

    private void UpdateCameraSensitivity()
    {
        if (freeLookCamera != null)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = _currentSensitivity * 600f;
            freeLookCamera.m_YAxis.m_MaxSpeed = _currentSensitivity * 4f;
        }
    }

    private void UpdateSensitivityText()
    {
        if (sensitivityText != null)
            sensitivityText.text = "Sensitivity:"+_currentSensitivity.ToString("F1"); // Form�t na 1 desetinn� m�sto
    }
}