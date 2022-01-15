using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private const float BALL_VELOCITY_MIN_AXIS_VALUE = 0.5f;
    
    [SerializeField]
    private float _initSpeed = 8;
    [SerializeField]
    private float _minSpeed = 6;
    [SerializeField]
    private float _maxSpeed = 10;
    
    private Rigidbody2D _rb;
    private Collider2D _collider;

    public void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        
        _collider.enabled = true;
        _rb.velocity = Random.insideUnitCircle.normalized * _initSpeed;
    }
    
    void FixedUpdate()
    {
        CheckVelocity();
    }
    
    private void CheckVelocity()
    {
        Vector2 velocity = _rb.velocity;
        float currentSpeed = velocity.magnitude;
        
        if (currentSpeed < _minSpeed)
        {
            velocity = velocity.normalized * _minSpeed;
        }
        else if (currentSpeed > _maxSpeed)
        {
            velocity = velocity.normalized * _maxSpeed;
        }
        
        if(Mathf.Abs(velocity.x) < BALL_VELOCITY_MIN_AXIS_VALUE) 
        {
            float sign = velocity.x == 0 ? Mathf.Sign(-transform.position.x) : Mathf.Sign(velocity.x);
            velocity.x += sign * BALL_VELOCITY_MIN_AXIS_VALUE * Time.deltaTime;
        }
        else if (Mathf.Abs(velocity.y) < BALL_VELOCITY_MIN_AXIS_VALUE)
        {
            float sign = velocity.y == 0 ? Mathf.Sign(-transform.position.y) : Mathf.Sign(velocity.y);   
            velocity.y += sign * BALL_VELOCITY_MIN_AXIS_VALUE * Time.deltaTime;
        }

        _rb.velocity = velocity;
    }
    
    // Esta funcion es invocada por Unity cuando se produce una colision
    private void OnCollisionEnter2D(Collision2D other)
    {
        BlockTile blockTileHit;
        if (!other.collider.TryGetComponent(out blockTileHit))
        {
            return;
        }

        ContactPoint2D contactPoint = other.contacts[0];
        blockTileHit.OnHitCollision(contactPoint);
    }

    public void Hide()
    {
        _collider.enabled = false;
        gameObject.SetActive(false);
    }
    
    public void ChangeBallSpeed(PowerUpKind kind)
    {   
        if (kind == PowerUpKind.FastBall)
        {
            _maxSpeed *= 2;
            _rb.velocity *= _rb.velocity.normalized * _maxSpeed;
        }
        else if (kind == PowerUpKind.SlowBall)
        {
            _minSpeed /= 2;
            _rb.velocity *= _rb.velocity.normalized * _minSpeed;
        }
    }

    public void ResetBallSpeed()
    {
        _maxSpeed = 14;
        _minSpeed = 2;
        _rb.velocity = _rb.velocity.normalized * _initSpeed;
    }
}