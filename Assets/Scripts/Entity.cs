using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    private KnobsAsset.Knob knob;

    [SerializeField]
    private Vector3 rotationChange;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float multiplier = 1f;

    private float speed = 0f;

    GameMgr gameMgr;

    private void Awake()
    {
        gameMgr = FindObjectOfType<GameMgr>();
        knob.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnDeselected()
    {
        knob.gameObject.SetActive(false);
    }

    public void OnSelected()
    {
        knob.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        knob.OnChangeEvent += OnKnobChanged;
    }

    private void OnDisable()
    {
        knob.OnChangeEvent -= OnKnobChanged;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Euler(rotationChange * time * 360f);
        target.transform.Rotate(Vector3.forward, speed * multiplier);
    }
    public void OnKnobChanged(float time)
    {
        speed = time * 2.0f - 1.0f;
        //Debug.Log("OnKnobChanged: " + time);
    }
}
