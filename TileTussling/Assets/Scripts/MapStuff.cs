using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapStuff
{
    public enum TileType {empty, grass, sand};

    [System.Serializable]
    public struct Map
    {
        [System.Serializable]
        public struct TileRow
        {
            public List<TileType> tiles;
        }

        public List<TileRow> matrix;

        public TileType getTile(int x, int y)
        {
            return matrix[y].tiles[x];
        }
    }
}