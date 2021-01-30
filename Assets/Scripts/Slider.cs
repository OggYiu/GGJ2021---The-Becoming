using UnityEngine;
using UnityEngine.Events;

namespace Presentation.Input
{
    public class Slider : TimeCrank
    {
        public System.Action<float> BeginChangedEvent;
        public System.Action<float> ChangedEvent;
        public System.Action<float> EndChangedEvent;

        protected bool _isDragging = false;

        [SerializeField] protected BoxCollider _touchCollider;
        [SerializeField] protected Transform _handle;
        [SerializeField] protected Transform _track;

        [SerializeField] private float _width = 2f;
        [SerializeField] private float _height = 2f;
        [SerializeField] private float _currentValue = 0f;
        [SerializeField] private float _lastValue = 0f;
        [SerializeField] private Camera _camera;

        public float Value => _currentValue;

        public bool IsDragging => _isDragging;

        public void SetValue(float percentValue)
        {
            percentValue = Mathf.Clamp(percentValue, 0f, 1f);
            SetHandlePosition(percentValue);
            OnValueChanged(percentValue);
        }

        public void SetValueWithoutTriggerEvent(float percentValue)
        {
            percentValue = Mathf.Clamp(percentValue, 0f, 1f);
            SetHandlePosition(percentValue);
        }

        public void OnMouseDown()
        {
            OnDragBegin();
        }

        private void OnMouseUp()
        {
            OnDragEnd();
        }

        public virtual void OnDragBegin()
        {
            _isDragging = true;

            BeginChangedEvent?.Invoke(_currentValue);
        }

        public virtual void OnDragEnd()
        {
            _isDragging = false;

            EndChangedEvent?.Invoke(_currentValue);
        }

        public void SetCamera(Camera camera)
        {
            _camera = camera;
        }
        
        protected void OnValueChanged(float percentage)
        {
            percentage = Mathf.Clamp(percentage, 0f, 1f);

            ChangedEvent?.Invoke(percentage);
        }

        protected Vector3 MousePositionOnRelativePlane()
        {
            Plane plane = new Plane(transform.up, transform.position);
            Ray ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (plane.Raycast(ray, out float hitDist))
            {
                Vector3 targetPoint = ray.GetPoint(hitDist);
                return targetPoint;
            }
            return Vector3.zero;
        }


        [ContextMenu("Apply Width & Height")]
        private void Apply()
        {
            _touchCollider.center = Vector3.zero;
            _touchCollider.size = new Vector3(_width, 0.01f, _height);
            _handle.localScale = new Vector3(_width, _height * .1f, 1f);
            _track.localScale = new Vector3(_width, _height, 1f);
        }

        private void Start()
        {
            if (_height < 0f)
            {
                Debug.LogWarning("Movement range should be positive", this);
            }

            if (_currentValue < 0f || _currentValue > _height)
            {
                Debug.LogWarning("Amount moved should be within the movement range", this);
            }

            _currentValue = Mathf.Clamp(_currentValue, 0, _height);

            SetPositionBasedOnCurrentValue();

            //float positionPercentage = _currentValue / _height;
            //OnValueChanged(positionPercentage);
        }

        private void Update()
        {
            if (_isDragging)
            {
                Vector3 mousePos = MousePositionOnRelativePlane();
                Vector3 mousePosOnAxis = PositionOnXAxisClosestToPoint(mousePos);
                float distance = Vector3.Distance(mousePosOnAxis, _handle.position);
                float dot = Vector3.Dot(transform.forward, mousePosOnAxis - _handle.position);
                _currentValue += (distance * (dot < 0f ? -1f : 1f));

                _currentValue = Mathf.Clamp(_currentValue, 0f, _height);

                SetPositionBasedOnCurrentValue();

                float positionPercentage = _currentValue / _height;

                bool isMoved = Mathf.Abs(_currentValue - _lastValue) >= 0.01f;

                if (isMoved) { OnValueChanged(positionPercentage); }

                _lastValue = _currentValue;
            }
        }

        protected void SetHandlePosition(float percentValue)
        {
            _currentValue = Mathf.Lerp(0f, _height, percentValue);
            SetPositionBasedOnCurrentValue();
        }

        private void SetPositionBasedOnCurrentValue()
        {
            Vector3 minPosition = (Vector3.forward * _height * -0.5f);
            _handle.localPosition = minPosition + (Vector3.forward * _currentValue);
        }

        private Vector3 PositionOnXAxisClosestToPoint(Vector3 point)
        {
            Ray xAxis = new Ray(transform.position, transform.forward);
            return xAxis.origin + xAxis.direction * Vector3.Dot(xAxis.direction, point - xAxis.origin);
        }

        override public void RegisterEvent(ITimeChild tc)
        {
            BeginChangedEvent += tc.OnBeginTimeChanged;
            ChangedEvent += tc.OnTimeChanged;
            EndChangedEvent += tc.OnEndTimeChanged;
        }

        override public void UnRegisterEvent(ITimeChild tc)
        {
            BeginChangedEvent -= tc.OnBeginTimeChanged;
            ChangedEvent -= tc.OnTimeChanged;
            EndChangedEvent -= tc.OnEndTimeChanged;
        }

        override public void OnTimeChildChanged(float percentage)
        {
            SetValueWithoutTriggerEvent(percentage);
        }

        override public void SetPercentage(float percentage)
        {
            SetValueWithoutTriggerEvent(percentage);
        }
    }
}
