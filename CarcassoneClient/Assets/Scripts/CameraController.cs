using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет камерой
/// </summary>
public class CameraController : MonoBehaviour {
    
    private Vector3 _mouseOrigin;
    private Vector3 _cameraOrigin;

    void Start ()
    {
    }

    void Update ()
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
}
