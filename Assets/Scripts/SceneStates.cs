using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitingModeState : AbstractSceneState
{
    public WaitingModeState(SceneController controller) : base(controller)
    {
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Q))
        {
            _controller.SetNextState();
        }
    }

    public override void OnEnter()
    {
        _controller.SetBackScreenContent("BackScreenBackground");
        _controller.SetFrontScreenContent("FrontScreenContentWM");
        
        _controller.FrontWM.SetOpaque();
    }

    public override void OnExit()
    {
        _controller.FrontWM.StartFadeOut();
        _controller.FrontPM.StartFadeIn();
    }
    
}

public class PlayModeState : AbstractSceneState
{
    private List<bool> _kernStates = new List<bool>(){false, false, false};
 
    private UnityEvent<int> _kernDownEvent = new UnityEvent<int>();
    private UnityEvent<int> _kernUpEvent = new UnityEvent<int>();

    private bool _readyMessageShown;
    private bool _isTransitionStarted;

    private void OnKernDown(int kernIndex)
    {
        _controller.SetKernState(kernIndex, true);
        _kernStates[kernIndex] = true;
    }

    private void OnKernUp(int kernIndex)
    {
        _controller.SetKernState(kernIndex, false);
        _kernStates[kernIndex] = false;
    }
    
    
    public PlayModeState(SceneController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        _kernDownEvent.AddListener(OnKernDown);
        _kernUpEvent.AddListener(OnKernUp);

        _readyMessageShown = false;
        _isTransitionStarted = false;
    }
    
    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.W)) _kernDownEvent.Invoke(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.E)) _kernDownEvent.Invoke(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.R)) _kernDownEvent.Invoke(2);
        
        if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.W)) _kernUpEvent.Invoke(0);
        if (Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.E)) _kernUpEvent.Invoke(1);
        if (Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.R)) _kernUpEvent.Invoke(2);

        if (!_kernStates.Contains(false) && !_readyMessageShown)
        {
            _controller.ShowReadyMessage();
            _readyMessageShown = true;
        }
        else if(_kernStates.Contains(false))
        {
            _controller.HideReadyMessage();
            _readyMessageShown = false;
        }

        if ((_readyMessageShown && Input.GetKeyDown(KeyCode.Alpha0) && !_isTransitionStarted) ||
            (_readyMessageShown && Input.GetKeyDown(KeyCode.Q) && !_isTransitionStarted))
        {
            Debug.Log("transitionStarted");
            _controller.TransitionedNextState(3);
            _controller.HideReadyMessage();
            _isTransitionStarted = true;
        }
    }
    
    public override void OnExit()
    {
    }
 
}

public class ContentMode : AbstractSceneState
{
    public ContentMode(SceneController controller) : base(controller)
    {
        
    }

    public override void OnEnter()
    {
        _controller.PlayModeLayoutFront.SetTransparent();
        _controller.ShowModeLayoutFront.SetOpaque();
        _controller.StartShowFront();
        _controller.SetBackScreenContent("show");
    }

    public override void OnExit()
    {
        _controller.ShowModeLayoutFront.SetTransparent();
    }
    
}