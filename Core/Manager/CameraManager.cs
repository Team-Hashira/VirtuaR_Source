using DG.Tweening;
using Hashira;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private AYellowpaper.SerializedCollections.SerializedDictionary<string, CinemachineCamera> _cameraDictionary = new AYellowpaper.SerializedCollections.SerializedDictionary<string, CinemachineCamera>();


    private Sequence _shakeSequence;

    private CinemachineVirtualCameraBase _currentCamera;
    public CinemachineVirtualCameraBase currentCamera
    {
        get
        {
            if (_currentCamera == null)
            {
                CinemachineVirtualCameraBase currentCam = CinemachineCore.GetVirtualCamera(0);
                _currentCamera = currentCam;
            }

            return _currentCamera;
        }
        private set
        {
            _currentCamera = value;
            currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    private CinemachineBasicMultiChannelPerlin _currentMultiChannel;
    private CinemachineBasicMultiChannelPerlin currentMultiChannel
    {
        get
        {
            if (_currentMultiChannel == null)
                _currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            return _currentMultiChannel;
        }
        set => _currentMultiChannel = value;
    }

    public void MoveToPlayerPositionimmediately()
    {
        currentCamera.ForceCameraPosition(currentCamera.Follow.position, Quaternion.identity);
    }

    public void ChangeCamera(CinemachineCamera camera)
    {
        currentMultiChannel.AmplitudeGain = 0;
        currentMultiChannel.FrequencyGain = 0;

        currentCamera.Priority = 10;
        currentCamera = camera;
        currentCamera.Priority = 11;
        currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void ChangeCamera(string cameraName)
    {
        currentMultiChannel.AmplitudeGain = 0;
        currentMultiChannel.FrequencyGain = 0;

        currentCamera.Priority = 10;
        currentCamera = _cameraDictionary[cameraName];
        currentCamera.Priority = 11;
        currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float amplitude, float frequency, float time, AnimationCurve curve, bool isAdd = true)
    {
        if (_shakeSequence != null && _shakeSequence.IsActive()) _shakeSequence.Kill();
        _shakeSequence = DOTween.Sequence();

        float startAmp;
        float startFre;
        if (isAdd)
        {
            startAmp = currentMultiChannel.AmplitudeGain + amplitude * OptionData.GraphicSaveData.screenEffectValue;
            startFre = currentMultiChannel.FrequencyGain + frequency * OptionData.GraphicSaveData.screenEffectValue;
        }
        else
        {
            startAmp = Mathf.Max(currentMultiChannel.AmplitudeGain, amplitude) * OptionData.GraphicSaveData.screenEffectValue;
            startFre = Mathf.Max(currentMultiChannel.FrequencyGain, frequency) * OptionData.GraphicSaveData.screenEffectValue;
        }

        _shakeSequence
            .Append(
                DOTween.To(() => startAmp,
                value => currentMultiChannel.AmplitudeGain = value,
                0, time).SetEase(curve))
            .Join(
                DOTween.To(() => startFre,
                value => currentMultiChannel.FrequencyGain = value,
                0, time).SetEase(curve));
    }
    public void ShakeCamera(float amplitude, float frequency, float time, Ease ease = Ease.Linear, bool isAdd = true)
    {
        if (_shakeSequence != null && _shakeSequence.IsActive()) _shakeSequence.Kill();
        _shakeSequence = DOTween.Sequence();

        float startAmp;
        float startFre;
        if (isAdd)
        {
            startAmp = currentMultiChannel.AmplitudeGain + amplitude * OptionData.GraphicSaveData.screenEffectValue;
            startFre = currentMultiChannel.FrequencyGain + frequency * OptionData.GraphicSaveData.screenEffectValue;
        }
        else
        {
            startAmp = Mathf.Max(currentMultiChannel.AmplitudeGain, amplitude) * OptionData.GraphicSaveData.screenEffectValue;
            startFre = Mathf.Max(currentMultiChannel.FrequencyGain, frequency) * OptionData.GraphicSaveData.screenEffectValue;
        }

        _shakeSequence
            .Append(
                DOTween.To(() => startAmp,
                value => currentMultiChannel.AmplitudeGain = value,
                0, time).SetEase(ease))
            .Join(
                DOTween.To(() => startFre,
                value => currentMultiChannel.FrequencyGain = value,
                0, time).SetEase(ease));
    }
}
