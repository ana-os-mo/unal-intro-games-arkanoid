using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private Vector2 _offset = new Vector2(-5.45f, 4);
    private Dictionary<int, BlockTile> _blockTiles = new Dictionary<int, BlockTile>();
    private LevelData _currentLevelData;

    public void BuildGrid(LevelData levelData)
    {
        _currentLevelData = levelData;
        ClearGrid();
        BuildGrid();
    }

    private void BuildGrid()
    {
        int id = 0;
        
        int rowCount = _currentLevelData.RowCount;
        float verticalSpacing = _currentLevelData.rowSpacing;

        for (int j = 0; j < rowCount; j++)
        {
            GridRowData rowData = _currentLevelData.Rows[j];
            
            int blockCount = rowData.BlockAmount;
            float horizontalSpacing = rowData.blockSpacing;
            Vector2 blockSize = GetBlockSize(rowData.BlockType);
            BlockTile blockTilePrefab = Resources.Load<BlockTile>(GetBlockPath(rowData.BlockType));
            BlockColor blockColor = rowData.BlockColor;

            if (blockTilePrefab == null)
            {
                return;
            }
            
            for (int i = 0; i < blockCount; i++)
            {
                BlockTile blockTile = Instantiate<BlockTile>(blockTilePrefab, transform);
                float x = _offset.x + blockSize.x/2 + (blockSize.x + horizontalSpacing) * i;
                float y = _offset.y - (blockSize.y + verticalSpacing) * j;
                blockTile.transform.position = new Vector3(x, y, 0);

                blockTile.SetData(id, blockColor);
                blockTile.Init();

                _blockTiles.Add(id, blockTile);
                id++;
            }
        }
    }

    private void ClearGrid()
    {
        int totalChildren = transform.childCount;
        for (int i = totalChildren - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        _blockTiles.Clear();
    }

    private Vector2 GetBlockSize(BlockType type)
    {
        if (type == BlockType.Big || type == BlockType.Small)
        {
            return new Vector2(1.5f, 0.5f);
        }

        return Vector2.zero;
    }

    private string GetBlockPath(BlockType type)
    {
        if (type == BlockType.Big)
        {
            return "Prefabs/BigBlockTile";
        }
        else if (type == BlockType.Small)
        {
            return "Prefabs/SmallBlockTile";
        }

        return string.Empty;
    }

    public int GetBlocksActive()
    {
        int totalActiveBlocks = 0;
        foreach (BlockTile block in _blockTiles.Values)
        {
            if (block.gameObject.activeSelf)
            {
                totalActiveBlocks++;
            }
        }

        return totalActiveBlocks;
    }

    public BlockTile GetBlockBy(int id)
    {
        if (_blockTiles.TryGetValue(id, out BlockTile block))
        {
            return block;
        }

        return null;
    }
}