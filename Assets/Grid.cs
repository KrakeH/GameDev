using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;

public class Grid : MonoBehaviour
{
    [SerializeField] private float scale=.1f;
    [SerializeField] private Vector2Int size;
    [SerializeField] private Level[] _levels;
    [SerializeField] private float _perliNoiseScale = 0.1f;
    private Cell[,] _grid;

    void Start()
    {
        float[,] noiseMap = new float[size.x, size.y];
        for (int y = 0; y < size.x; y++)
        {
            for (int x = 0; x < size.y; x++)
            {
                noiseMap[x, y] = Mathf.PerlinNoise(_perliNoiseScale * y, _perliNoiseScale * x);
            }
        }
        _grid = new Cell[size.x, size.y];
        for (int y = 0; y < size.x; y++)
        {
            for (int x = 0; x < size.y; x++)
            {
                float noiselevel = noiseMap[y, x];
                Cell cell = new Cell();
                foreach(Level level in _levels)
                {
                    if (noiselevel < level.maxlevel)
                    {
                        cell.LandType = level.landType;
                        break;
                    }
                };
                _grid[y, x] = cell;
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell cell = _grid[i, j];
                switch (cell.LandType)
                {
                    case LandType.Grass:
                        Gizmos.color = Color.green;
                        break;
                    case LandType.Water:
                        Gizmos.color = Color.blue;
                        break;
                    case LandType.Rock:
                        Gizmos.color = Color.gray;
                        break;
                }

                Vector3 pos = new Vector3(j, 0, i);
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }
    }

    [Serializable]
    private class Level
    {
        public float maxlevel;
        public LandType landType;
    }
}