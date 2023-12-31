using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public enum VolumeType
{
    MASTER,
    BGM,
    SE
}

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private VolumeType volumeType;

    public float SliderVolume
    {
        get { return slider.value; }
    }

    public VolumeType VolumeType
    {
        get { return volumeType; }
    }

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        var mixer = AudioManager.Instance.Mixer;
        string type = VolumeType.ToString() + "Vol";
        float volume;

        slider.onValueChanged.AddListener(ChangeValue);
        slider.minValue = 0.0001f;
        slider.maxValue = 1.0f;

        mixer.GetFloat(type, out volume);
        slider.value = Mathf.Pow(10, (volume / 20));
    }

    private void ChangeValue(float value)
    {
        //Debug.Log(VolumeType.ToString() + "Vol" + " Changed!");
        var mixer = AudioManager.Instance.Mixer;
        string type = VolumeType.ToString() + "Vol";
        float volume = Mathf.Log10(value) * 20;

        mixer.SetFloat(type, volume);
    }
}
