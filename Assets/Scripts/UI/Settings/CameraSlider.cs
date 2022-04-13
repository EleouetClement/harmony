using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CameraSlider : MonoBehaviour
{
    Slider slider;
    CinemachineCameraController cameraController;
    ControlsSettingsSaver controlsSettingsSaver;

    // Start is called before the first frame update
    void Start()
    {
        controlsSettingsSaver = ControlsSettingsSaver.Instance;
        slider = GetComponent<Slider>();
        cameraController = GameModeSingleton.GetInstance().GetCinemachineCameraController;
        var range = typeof(CinemachineCameraController).GetField(nameof(CinemachineCameraController.sensibility)).GetCustomAttribute<RangeAttribute>();
        slider.maxValue = range.max;
        slider.minValue = range.min;
    }

    // Update is called once per frame
    void Update()
    {
        slider.onValueChanged.AddListener(ChangeSensitivity);
    }

    void ChangeSensitivity(float val)
    {
        cameraController.sensibility = val;
        ControlsSettingsSaver.Instance.cameraSensitivity = val;
    }
}
