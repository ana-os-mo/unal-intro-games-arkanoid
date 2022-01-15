using UnityEngine;
using Random = UnityEngine.Random;

public enum PowerUpKind
{
    LargePaddle,
    SmallPaddle,
    MultiBall,
    SlowBall,
    FastBall
}

public class PowerUps : MonoBehaviour
{
    private const string POWER_UP_PATH = "Sprites/PowerUps/PowerUp_{0}";

    private PowerUpKind _kind = PowerUpKind.LargePaddle;
    // getter para llamarlo desde otro lado
    public PowerUpKind Kind => _kind;

    private Rigidbody2D _rb;
    private Collider2D _collider;
    private SpriteRenderer _renderer;

    public void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _collider = GetComponent<Collider2D>();
        _collider.enabled = true;
        
        _renderer = GetComponentInChildren<SpriteRenderer>();

        // powerUp aleatorio del enum para renderizar
        _kind = (PowerUpKind)Random.Range(0, 5);
        _renderer.sprite = GetPowerUpSprite(_kind);
    }

    static Sprite GetPowerUpSprite(PowerUpKind kind)
    {
        string path = string.Empty;
        path = string.Format(POWER_UP_PATH, kind);

        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return Resources.Load<Sprite>(path);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("PowerUp hit " + other.name);
        if (other.name == "Paddle")
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
            ArkanoidEvent.OnPowerUpPaddleContactEvent?.Invoke(this);
        }
    }
}
