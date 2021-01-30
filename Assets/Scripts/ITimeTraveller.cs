using UnityEngine;

public interface ITimeBridge
{
    ITimeChildEventReceiver GetReceiver();
    ITimeChild GetTimeChild();

}

public interface ITimeChildEventReceiver
{
    void OnTimeChildChanged(float percentage);
    void SetPercentage(float percentage);
}

public interface ITimeEvaluator
{
    void Evaluate(float percentage);
}

public interface ITimeCrank
{
    void RegisterEvent(ITimeChild tc);
    void UnRegisterEvent(ITimeChild tc);
}
public interface ITimeChild
{
    float CurrentTime();
    float MaxTime();
    void SetTarget(GameObject target);
    GameObject Target();

    void Pause();
    void UnPause();
    void Freeze();
    void UnFreeze();
    void Reset();
    void Step(float time);
    void Evaluate();
    ITimeEvaluator Evaluator();
    void SetTime(float time);
    void SetTimeDiff(float time);
    void OnBeginTimeChanged(float percentage);
    void OnTimeChanged(float percentage);
    void OnEndTimeChanged(float percentage);
    bool IsFreezed();
    bool IsPaused();
    void RegisterEvent(ITimeChildEventReceiver tcer);
    void UnRegisterEvent(ITimeChildEventReceiver tcer);
}
