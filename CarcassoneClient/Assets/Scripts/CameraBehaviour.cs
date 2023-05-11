using UnityEngine;
using Assets.Scripts;

enum TouchState
{
    None = 0,
    Drag = 1,
    Zoom = 2,
}

/// <summary>
/// Управляет камерой
/// </summary>
public class CameraBehaviour : MonoBehaviour {
    
    private Vector3 _mouseOrigin;
    private Vector3 _cameraOrigin;

    private TouchState _state;

    void Start ()
    {
    }

    void Update ()
    {
        if (Input.touchCount > 0)
        {
            DragByTouch();
            ZoomByTouch();
        }
        else
        {
            _state = TouchState.None;
            Drag();
        }
    }

    private void DragByTouch()
    {
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began) // запоминаем где нажали кнопку
            {
                _state = TouchState.Drag;
                _mouseOrigin = Input.touches[0].position;
                _cameraOrigin = transform.position;
                return;
            }

            if (_state != TouchState.Drag)
                return;

            var deltaVec = Camera.main.ScreenToWorldPoint(_mouseOrigin) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 move = _cameraOrigin + new Vector3(deltaVec.x, deltaVec.y, 0);
            transform.position = move;
        }
    }

    private void Drag()
    {
        if (Input.GetMouseButtonDown(0)) // запоминаем где нажали кнопку
        {
            _mouseOrigin = Input.mousePosition;
            _cameraOrigin = transform.position;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        var deltaVec = Camera.main.ScreenToWorldPoint(_mouseOrigin) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 move = _cameraOrigin + new Vector3(deltaVec.x, deltaVec.y, 0);
        transform.position = move;
    }

    private float _zoomModifierSpeed = 0.025f;
    private void ZoomByTouch()
    {
        if (Input.touchCount == 2)
        {
            _state = TouchState.Zoom;

            var firstTouch = Input.GetTouch(0);
            var secondTouch = Input.GetTouch(1);

            if (firstTouch.position.x < 5 || firstTouch.position.x > Screen.width - 5)
                return;

            if (secondTouch.position.x < 5 || secondTouch.position.x > Screen.width - 5)
                return;

            var firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            var secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            var touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            var touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            var zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * _zoomModifierSpeed;

            if (touchesPrevPosDifference > touchesCurPosDifference)
                Camera.main.orthographicSize += zoomModifier;

            if (touchesPrevPosDifference < touchesCurPosDifference)
                Camera.main.orthographicSize -= zoomModifier;
        }

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2, 10);
    }
}
