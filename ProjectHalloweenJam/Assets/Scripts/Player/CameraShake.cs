using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _shakeIntensity = 1f;
    [SerializeField] private float _shakeTime = 0.2f;

    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _channelPerlin;

    private void Awake() 
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _channelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake() => StartCoroutine(ShakeCamera());

    private IEnumerator ShakeCamera()
    {
        _channelPerlin.m_AmplitudeGain = _shakeIntensity;
        yield return new WaitForSeconds(_shakeTime);
        _channelPerlin.m_AmplitudeGain = 0f;
    }
}
