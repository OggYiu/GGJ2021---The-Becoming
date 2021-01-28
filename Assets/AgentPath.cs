using Shapes;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class AgentPath : MonoBehaviour
{
    [SerializeField] private CubicHermiteSpline spline;
    private Agent agent;

    private void Awake()
    {
        agent = GetComponent<Agent>();
    }

    void Start()
    {
    }

    private void OnEnable()
    {
        agent.OnTimeChange += OnTimeChange;
    }

    private void OnDisable()
    {
        agent.OnTimeChange -= OnTimeChange;
    }

    private void OnTimeChange(float value)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Draw.Line(transform.position, transform.forward * 1.0f);
    }
}
