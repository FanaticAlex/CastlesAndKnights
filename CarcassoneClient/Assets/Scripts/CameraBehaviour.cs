using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет камерой
/// </summary>
public class CameraBehaviour : MonoBehaviour {
    
    private Vector3 _mouseOrigin;
    private Vector3 _cameraOrigin;

    void Start ()
    {
    }

    void Update ()
    {
        Drag();
        Zoom();
    }

    private void Drag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseOrigin = Input.mousePosition;
            _cameraOrigin = transform.position;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        var delta = Camera.main.ScreenToWorldPoint(_mouseOrigin) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 deltaVec = delta;
        Vector3 move = new Vector3(_cameraOrigin.x + deltaVec.x, _cameraOrigin.y + deltaVec.y, _cameraOrigin.z);
        transform.position = move;
    }

    private float _zoomModifierSpeed = 0.025f;
    private void Zoom()
    {
        if (Input.touchCount == 2)
        {
            var firstTouch = Input.GetTouch(0);
            var secondTouch = Input.GetTouch(1);

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
