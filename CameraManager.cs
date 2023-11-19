using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;
    [Header("Controls for lerping the Y Damping during player jump/fal")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float _fallSpeedYDampingChangeThreshold = -15f;

    public bool isLerypingYDamping { get; private set; }

    public bool lerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpYPanCoroutine;

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;

    private float _normYPanAmount;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i=0; i<_allVirtualCameras.Length; i++)
        {
            if (_allVirtualCameras[i].enabled)
            {
                //set the current active camera
                _currentCamera = _allVirtualCameras[i];

                //set the framing transposer
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();



            }
        }
        _normYPanAmount = _framingTransposer.m_YDamping;
    }


    #region Lerp the Y damping
    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));

    }

    private IEnumerator LerpYAction(bool isPlayerfalling)
    {
        isLerypingYDamping = true;

        //grab the starting damping amount
        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0f;
         
        //determine th end damping amount
        if (isPlayerfalling)
        {
            endDampAmount = _fallPanAmount;
            lerpedFromPlayerFalling = true;

        }
        else
        {
            endDampAmount = _normYPanAmount;
        }

        float elapsedTime = 0f;
        while(elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.time;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime);
            yield return null;
        }
        isLerypingYDamping = false;
    }
    #endregion
}
