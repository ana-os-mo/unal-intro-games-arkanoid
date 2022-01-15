using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    private float _speed = 7;
    [SerializeField]
    private float _movementLimit = 7;

    private Vector3 _targetPosition;

    private Camera _cam;
    private Camera Camera
    {
        get
        {
            if (_cam == null)
            {
                _cam = Camera.main;
            }
            return _cam;
        }
    }
    
    void Update()
    {
        _targetPosition.x = Camera.ScreenToWorldPoint(Input.mousePosition).x;
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, -_movementLimit, _movementLimit);
        _targetPosition.y = this.transform.position.y;

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _speed);
    }

    public void ScalePaddle(PowerUpKind kind)
    {   
        if (kind == PowerUpKind.LargePaddle)
        {   
            this.transform.localScale += new Vector3(0.3f, 0.0f);
        }
        else if (kind == PowerUpKind.SmallPaddle)
        {
            this.transform.localScale -= new Vector3(0.3f, 0.0f);
        }
    }

    public void ResetPaddle()
    {
        this.transform.localScale = new Vector3(1.0f, 1.0f);
    }
}
