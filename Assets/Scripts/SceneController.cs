using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using AwakeSolutions;
using UnityEngine;
using UnityEngine.Serialization;

public class SceneController : MonoBehaviour
{
    private List<AbstractSceneState> _sceneStates;
    private int _currentStateIndex;
    private AbstractSceneState _currentState;
    
    [Header("Media Players")]
    public string VideosFolder;
    [SerializeField] private AwakeMediaPlayer[] _kernStands;
    
    [Header("Layers Alpha")] 
    public AlphaTransition ReadyMessage;

    [Header("Layer Controllers")] 
    public LayerController BackScreenContent;
    public LayerController BackScreenTransition;

    public LayerController FrontScreenWM;
    public LayerController FrontScreenPM;
    public LayerController FrontScreenShowMode;
    public LayerController FrontScreenTransition;
    

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
        FrontScreenTransition.SetOpaque();
        FrontScreenTransition.Play();
        yield return new WaitForSeconds(duration - 0.3f);
        SetNextState();
        yield return new WaitForSeconds(0.3f);
        FrontScreenTransition.SetTransparent();
    }

    private IEnumerator ShowTransitionBack(float duration)
    {
        BackScreenTransition.SetOpaque();
        BackScreenTransition.Play();
        yield return new WaitForSeconds(duration);
        BackScreenTransition.SetTransparent();
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

    public void SetKernState(int kernId, bool state)
    {
        string filename = state ? "KernGreen" : "KernRed";
        _kernStands[kernId].Open(VideosFolder, filename, true, true);
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

    #endregion
    
}
