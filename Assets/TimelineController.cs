using Presentation.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class TimelineController : MonoBehaviour
{
    [SerializeField] private List<PlayableDirector> playableDirectors;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        slider.BeginChangedEvent += OnSliderBeginChanged;
        slider.ChangedEvent += OnSliderValueChanged;
        slider.EndChangedEvent += OnSliderEndChanged;
    }

    private void OnDisable()
    {
        slider.BeginChangedEvent -= OnSliderBeginChanged;
        slider.ChangedEvent -= OnSliderValueChanged;
        slider.EndChangedEvent -= OnSliderEndChanged;
    }

    private void OnSliderBeginChanged(float percentage)
    {
        playableDirectors[0].Pause();
    }

    private void OnSliderValueChanged(float percentage)
    {
        //Debug.Log("OnSliderValueChanged: ")
        playableDirectors[0].time = playableDirectors[0].duration * percentage;
        playableDirectors[0].Evaluate();
        //playableDirectors[0].st
        UpdateTimeDisplay();
    }

    private void UpdateTimeDisplay()
    {
        text.text = "Hour: " + Mathf.Floor((float)(playableDirectors[0].time / playableDirectors[0].duration) * 24.0f);
    }

    private void OnSliderEndChanged(float percentage)
    {
        playableDirectors[0].Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if(!slider.IsDragging)
        {
            slider.SetValueWithoutTriggerEvent((float)(playableDirectors[0].time / playableDirectors[0].duration));
            UpdateTimeDisplay();
        }
    }
}
