using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private PlayerController PlayerController;

    private const string SensitivityKey = "Sensitivity";

    private void Start() {
        float savedValue = PlayerPrefs.GetFloat(SensitivityKey, 5f);
        sensitivitySlider.value = savedValue;

        sensitivitySlider.onValueChanged.AddListener((v) => {
            PlayerController.mouseSensitivity = v;
            PlayerPrefs.SetFloat(SensitivityKey, v);
            PlayerPrefs.Save();
        });
    }
}
