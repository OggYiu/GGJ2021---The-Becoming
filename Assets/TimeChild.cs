using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITimeEvaluator))]
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

    private float stepDirection = 1f;

    protected GameObject _target;

    public Action<float> OnTimeChangedEvent;

    public float CurrentTime() => _currentTime;
    public float MaxTime() => _maxTime;

    public bool IsFreezed() => _freezed;

    public bool IsPaused() => _paused;

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
        if (time < 0) time = 0;
        if (time > MaxTime()) time = MaxTime();
        _currentTime = time;

        OnTimeChangedEvent?.Invoke(CurrentTime() / MaxTime());

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
        float time = _currentTime + dt * stepDirection;

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
                    stepDirection = 1.0f;
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
                    stepDirection = 1.0f;
                }
                break;
            case StepMethod.PingPong:
                if (time > MaxTime())
                {
                    time = 2.0f * MaxTime() - time;
                    stepDirection = -1.0f;
                }
                else if (time < 0)
                {
                    time %= MaxTime();
                    time = time * -1.0f;
                    stepDirection = 1.0f;
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
        GetComponent<ITimeEvaluator>().Evaluate(CurrentTime() / MaxTime());
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
    }

    public void RegisterEvent(ITimeChildEventReceiver tcer)
    {
        OnTimeChangedEvent += tcer.OnTimeChildChanged;
    }

    public void UnRegisterEvent(ITimeChildEventReceiver tcer)
    {
        OnTimeChangedEvent -= tcer.OnTimeChildChanged;
    }
}
