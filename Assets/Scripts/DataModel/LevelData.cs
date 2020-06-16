using UnityEngine;

namespace DataModel
{
    public class LevelData
    {
        public int levelId;

        public int chapter;

        public int index;

        public string itemId;
        
        public Vector2 position
        {
            get
            {
                int gridIndex = index + 1;
                int row = gridIndex / 7;
                int column = gridIndex % 7;
//                Vector2 pos = new Vector2(15+150*column,-205*row-135);
                Vector2 pos = new Vector2(-197 + 126 * column, -176 * row + 343);
                return pos;
            }
        }
    }
}