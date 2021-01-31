using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(ITimeEvaluator))]
public class TimeChild : MonoBehaviour, ITimeChild
{
    public enum StepMethod
    {
        Hold,
        Loop,
        PingPong,
    }

    [SerializeField] private float _currentTime = 0;
    [SerializeField] private float _maxTime = 1f;
    [SerializeField] private bool _freezed = false;
    [SerializeField] private bool _paused = false;
    [SerializeField] private TimeCrank timeCrank;
    [SerializeField] private StepMethod stepMethod;
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool showKnobWhenStart = false;
    [SerializeField] private bool clickToShowKnob = true;
    [SerializeField] private bool fadeAfterRelease = true;
    [SerializeField] private bool modifySpeedAfterDrag = false;
    [SerializeField] private float afterDragSpeedMultipler = 0.1f;

    protected TimeChildMgr timeChildMgr;

    protected GameObject _target;

    public Action<float> OnTimeChangedEvent;

    public float CurrentTime() { return _currentTime; }
    public float MaxTime() => _maxTime;

    public bool IsFreezed() => _freezed;

    public bool IsPaused() => _paused;

    private void Start()
    {
        timeCrank.gameObject.SetActive(showKnobWhenStart);
        timeChildMgr = FindObjectOfType<TimeChildMgr>();

        timeChildMgr.Add(this);
    }

    private void OnDestroy()
    {
        timeChildMgr.Remove(this);
    }

    private void OnEnable()
    {
        timeCrank.RegisterEvent(this);
    }

    private void OnDisable()
    {
        timeCrank.UnRegisterEvent(this);
    }

    public void Pause()
    {
        _paused = true;
    }

    public void Reset()
    {
        _currentTime = 0;
    }

    public void UnPause()
    {
        _paused = false;
    }

    public void SetTime(float time)
    {
        SetTimeWithoutFireEvent(time);

        OnTimeChangedEvent?.Invoke(CurrentTime() / MaxTime());
    }

    public void SetTimeWithoutFireEvent(float time)
    {
        if (time < 0) time = 0;
        if (time > MaxTime()) time = MaxTime();
        _currentTime = time;

        Evaluate();
    }

    public void SetTimeDiff(float time)
    {
        float t = _currentTime + time;
        if (t < 0) t = 0;
        if (t >= MaxTime()) t = MaxTime();
        SetTime(t);
    }

    public void Step(float dt)
    {
        if (_paused) return;
        if (_freezed) return;
        float time = _currentTime + dt * speed;

        switch(stepMethod)
        {
            case StepMethod.Hold:
                if (time > MaxTime())
                {
                    time = MaxTime();
                }
                else if (time < 0)
                {
                    time = 0;
                    speed = Math.Abs(speed);
                }
                break;
            case StepMethod.Loop:
                if (time > MaxTime())
                {
                    time %= MaxTime();
                }
                else if (time < 0)
                {
                    time = 0;
                    speed = Math.Abs(speed);
                }
                break;
            case StepMethod.PingPong:
                if (time > MaxTime())
                {
                    time = 2.0f * MaxTime() - time;
                    speed = -Math.Abs(speed);
                }
                else if (time < 0)
                {
                    time %= MaxTime();
                    time = time * -1.0f;
                    speed = Math.Abs(speed);
                }

                break;
        }

        SetTime(time);
    }

    // Update is called once per frame
    void Update()
    {
        Step(Time.deltaTime);
    }

    virtual public void Evaluate()
    {
        if(GetComponent<ITimeEvaluator>() != null)
        {
            GetComponent<ITimeEvaluator>().Evaluate(CurrentTime() / MaxTime());
        }
    }

    public void Freeze()
    {
        _freezed = true;
    }

    public void UnFreeze()
    {
        _freezed = false;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public GameObject Target() => _target;

    public ITimeEvaluator Evaluator()
    {
        throw new NotImplementedException();
    }

    public void OnTimeChanged(float percentage)
    {
        SetTime(percentage * MaxTime());
    }

    public void OnBeginTimeChanged(float percentage)
    {
        Pause();
    }

    public void OnEndTimeChanged(float percentage)
    {
        UnPause();

        if(fadeAfterRelease)
        {
            StartCoroutine(DeActiveTimeCrank());
        }

        if(modifySpeedAfterDrag)
        {
            speed *= afterDragSpeedMultipler;
        }
    }

    IEnumerator DeActiveTimeCrank()
    {
        yield return new WaitForSeconds(0.5f);
        timeCrank.gameObject.SetActive(false);
    }

    public void RegisterEvent(ITimeChildEventReceiver tcer)
    {
        OnTimeChangedEvent += tcer.OnTimeChildChanged;
    }

    public void UnRegisterEvent(ITimeChildEventReceiver tcer)
    {
        OnTimeChangedEvent -= tcer.OnTimeChildChanged;
    }

    private void OnMouseDown()
    {
        timeChildMgr.OnMouseDown(this);
    }

    public void OnSelected()
    {
        if(clickToShowKnob)
        {
            timeCrank.gameObject.SetActive(true);
        }
    }

    public void OnDeSelected()
    {
        if (clickToShowKnob)
        {
            timeCrank.gameObject.SetActive(false);
        }
    }
}
