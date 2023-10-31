using System.Collections;
using System.Collections.Generic;
using AwakeSolutions;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private List<AbstractSceneState> _sceneStates;
    private int _currentStateIndex;
    private AbstractSceneState _currentState;
    
    [Header("Media Players")]
    [SerializeField] private string _videosFolder;
    
    [SerializeField] private AwakeMediaPlayer _backScreenBackground;
    [SerializeField] private AwakeMediaPlayer _frontScreenContentWM;
    [SerializeField] private AwakeMediaPlayer _frontScreenContentPM;
    [SerializeField] private AwakeMediaPlayer _transitionMediaPlayerFront;
    [SerializeField] private AwakeMediaPlayer _transitionMediaPlayerBack;
    [SerializeField] private AwakeMediaPlayer _showModeMediaPlayerFront;
    [SerializeField] private AwakeMediaPlayer[] _kernStands;
    
    [Header("Layers Alpha")] 
    public AlphaTransition FrontWM;
    public AlphaTransition FrontPM;
    public AlphaTransition ReadyMessage;
    public AlphaTransition TransitionLayoutFront;
    public AlphaTransition TransitionLayoutBack;
    public AlphaTransition PlayModeLayoutFront;
    public AlphaTransition ShowModeLayoutFront;

    #region private methods

    private void Start()
    {
        _sceneStates = new List<AbstractSceneState>()
        {
            new WaitingModeState(this),
            new PlayModeState(this),
            new ContentMode(this)
        };

        _currentStateIndex = 0;
        _currentState = _sceneStates[_currentStateIndex];
        _currentState.OnEnter();
    }
    
    private void Update()
    {
        _currentState.OnUpdate();
    }
    
    private IEnumerator ShowTransitionFront(float duration)
    {
        TransitionLayoutFront.SetOpaque();
        _transitionMediaPlayerFront.Play();
        yield return new WaitForSeconds(duration - 0.3f);
        SetNextState();
        yield return new WaitForSeconds(0.3f);
        TransitionLayoutFront.SetTransparent();
        
    }

    private IEnumerator ShowTransitionBack(float duration)
    {
        TransitionLayoutBack.SetOpaque();
        _transitionMediaPlayerBack.Play();
        yield return new WaitForSeconds(duration);
        TransitionLayoutBack.SetTransparent();
    }

    #endregion


    #region public methods
    
    public void SetNextState()
    {
        if (_currentStateIndex + 1 < _sceneStates.Count)
        {
            _currentStateIndex++;
            _currentState.OnExit();

            _currentState = _sceneStates[_currentStateIndex];
            _currentState.OnEnter();
        }
        else
        {
            _currentStateIndex = 0;
            _currentState.OnExit();

            _currentState = _sceneStates[_currentStateIndex];
            _currentState.OnEnter();
        }
    }

    public void SetBackScreenContent(string filename)
    {
        _backScreenBackground.Open(_videosFolder, filename, true, true);
    }
    

    public void SetFrontScreenContent(string filename, bool onWM = true)
    {
        if (onWM)
        {
            _frontScreenContentWM.Open(_videosFolder, filename, true, true);
        }
        else
        {
            _frontScreenContentPM.Open(_videosFolder, filename, true, true);
        }
    }

    public void SetKernState(int kernId, bool state)
    {
        string filename = state ? "KernGreen" : "KernRed";
        _kernStands[kernId].Open(_videosFolder, filename, true, true);
    }

    public void ShowReadyMessage()
    {
        ReadyMessage.StartFadeIn();
    }

    public void HideReadyMessage()
    {
        ReadyMessage.StartFadeOut();
    }

    public void TransitionedNextState(float duration)
    {
        StartCoroutine(ShowTransitionBack(duration));
        StartCoroutine(ShowTransitionFront(duration));
    }

    public void StartShowFront()
    {
        _showModeMediaPlayerFront.Play();
    }

    #endregion
    
}
