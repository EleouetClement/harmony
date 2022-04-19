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

    bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        cameraController = GameModeSingleton.GetInstance().GetCinemachineCameraController;
        var range = typeof(CinemachineCameraController).GetField(nameof(CinemachineCameraController.sensibility)).GetCustomAttribute<RangeAttribute>();
        slider.maxValue = range.max;
        slider.minValue = range.min;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            if (ControlsSettingsSaver.Instance)
            {
                slider.value = ControlsSettingsSaver.Instance.dataHandler.CameraSensitivity;
                initialized = true;
            }
        }
        slider.onValueChanged.AddListener(ChangeSensitivity);
    }

    void ChangeSensitivity(float val)
    {
        cameraController.sensibility = val;
        ControlsSettingsSaver.Instance.cameraSensitivity = val;
    }
}
