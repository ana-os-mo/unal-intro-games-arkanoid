using UnityEngine;

public enum BlockType
{
    Small,
    Big
}

public enum BlockColor
{
    Green,
    Blue,
    Yellow,
    Red,
    Purple
}

public class BlockTile : MonoBehaviour
{
    private PowerUps _powerUpPrefab = null;

    private const string POWER_UP_PREFAB_PATH = "Prefabs/PowerUp";
    private const string BLOCK_BIG_PATH = "Sprites/BlockTiles/Big/Big_{0}_{1}";
    private const string BLOCK_SMALL_PATH = "Sprites/BlockTiles/Small/Small_{0}_1";
    
    [SerializeField] 
    private BlockType _type = BlockType.Big;
    [SerializeField] 
    private BlockColor _color = BlockColor.Yellow;
    [SerializeField]
    private int _score = 10;
    
    public int Score => _score;
    
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    
    private int _id;

    private int _totalHits = 1;
    private int _currentHits = 0;

    public void SetData(int id, BlockColor color)
    {
        _id = id;
        _color = color;
    }

    public void Init()
    {
        _currentHits = 0;
        _totalHits = _type == BlockType.Big ? 2 : 1;

        _collider = GetComponent<Collider2D>();
        _collider.enabled = true;
        
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _renderer.sprite = GetBlockSprite(_type, _color, 0);
    }
    
    // capsula.position = this.position;

    public void OnHitCollision(ContactPoint2D contactPoint)
    {
        _currentHits++;
        float rand = Random.value;
        if (_currentHits >= _totalHits)
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
            ArkanoidEvent.OnBlockDestroyedEvent?.Invoke(_id);

            if (rand <= 0.35)
            {
                Vector2 powerUpPosition = new Vector2(this.transform.position.x, this.transform.position.y);
                PowerUps powerUp = CreatePowerUpAt(powerUpPosition);
                powerUp.Init();
            }
        }
        else
        {
            _renderer.sprite = GetBlockSprite(_type, _color, _currentHits);
        }
    }

    private PowerUps CreatePowerUpAt(Vector2 position)
    {
        if (_powerUpPrefab == null)
        {
            _powerUpPrefab = Resources.Load<PowerUps>(POWER_UP_PREFAB_PATH);
        }
        return Instantiate(_powerUpPrefab, position, Quaternion.identity);
    }
    
    static Sprite GetBlockSprite(BlockType type, BlockColor color, int state)
    {
        string path = string.Empty;
        if (type == BlockType.Big)
        {
            path = string.Format(BLOCK_BIG_PATH, color, state);
        }
        else if (type == BlockType.Small)
        {
            path = string.Format(BLOCK_SMALL_PATH, color);
        }

        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return Resources.Load<Sprite>(path);
    }
}