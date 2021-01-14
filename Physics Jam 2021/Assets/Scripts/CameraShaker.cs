using Cinemachine;
using System.Collections;
using UnityEngine;

// https://github.com/Lumidi/CameraShakeInCinemachine

public class CameraShaker : MonoBehaviour
{
    [SerializeField] float Duration = 0.2f;
    [SerializeField] float Amplitude = 1.2f;
    [SerializeField] float Frequency = 2.0f;

    [SerializeField] CinemachineVirtualCamera VirtualCamera = null;

    private CinemachineBasicMultiChannelPerlin virtualCameraNoise = null;

    private void Start()
    {
        virtualCameraNoise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Reset()
    {
        Duration = 0.2f;
        Amplitude = 1.2f;
        Frequency = 2.0f;
    }

    public void Shake()
    {
        StartCoroutine(DoShake());
    }

    private IEnumerator DoShake()
    {
        virtualCameraNoise.m_AmplitudeGain = Amplitude;
        virtualCameraNoise.m_FrequencyGain = Frequency;

        yield return new WaitForSeconds(Duration);

        virtualCameraNoise.m_AmplitudeGain = 0f;
        virtualCameraNoise.m_FrequencyGain = 0f;
    }
}
