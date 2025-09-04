using UnityEngine;

public enum TouchState
{
    Drag,
    ZoomToCard
}

/// <summary>
/// Управляет камерой
/// </summary>
public class CameraBehaviour : MonoBehaviour {
    
    private Vector3 _mouseOrigin;
    private Vector3 _cameraOrigin;

    public TouchState State { get; set; }

    void Update ()
    {
        switch (State)
        {
            case TouchState.Drag: Drag(); break;
            case TouchState.ZoomToCard: AnimateToCard(); break;
        }

        FreeZoom();
    }

    public void MoveCameraAtCard(Vector3 cardPosition) // приближаем к карте
    {
        State = TouchState.ZoomToCard;
        _animationPosition = 0;
        _startPosition = Camera.main.transform.position;
        _endPosition = new Vector3(cardPosition.x, cardPosition.y, Camera.main.transform.position.z);
    }

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _animationPosition;
    private const float _animationTime = 1; // в секундах
    private void AnimateToCard()
    {
        _animationPosition += Time.deltaTime;
        var t = _animationPosition / _animationTime;
        Camera.main.transform.position = Vector3.Lerp(_startPosition, _endPosition, t);

        if (t >= 1) // окончание анимации
            State = TouchState.Drag;
    }

    private void Drag()
    {
        // пока происходит перемещение запоминаем исходную точку и вычисляем текущую
         
        if (Input.GetMouseButtonDown(0)) // PC
        {
            // инициализация, стартовая позиция при нажатии
            _mouseOrigin = Input.mousePosition;
            _cameraOrigin = transform.position;
            return;
        }

        if ((Input.touchCount == 1) && (Input.touches[0].phase == TouchPhase.Began)) // Mobile
        {
            // инициализация, стартовая позиция при нажатии
            _mouseOrigin = Input.touches[0].position;
            _cameraOrigin = transform.position;
            return;
        }

        if ((Input.touchCount != 1) && (!Input.GetMouseButton(0))) // перетаскивание прекращено
            return;

        var deltaVec = Camera.main.ScreenToWorldPoint(_mouseOrigin) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 move = _cameraOrigin + new Vector3(deltaVec.x, deltaVec.y, 0);
        transform.position = move;
    }

    private readonly float _zoomModifierSpeed = 0.025f;
    private void FreeZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // PC
        {
            Camera.main.orthographicSize += Input.GetAxis("Mouse ScrollWheel");
        }

        if (Input.touchCount == 2) // Mobile
        {
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
