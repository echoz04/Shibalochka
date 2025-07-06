using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Fish Config", menuName = "Configs/New Item Config")]
    public class ItemConfig : ScriptableObject
    {
        [field: SerializeField] public string TypeId { get; private set; }
        [field: SerializeField] public string TitleLid { get; private set; }
        [field: SerializeField] public Vector2Int[] CellOffSets { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        public Vector2 GetPivotOffset(bool rotated)
        {
            var offsets = CellOffSets;

            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;

            foreach (var offset in offsets)
            {
                Vector2Int rotatedOffset = rotated ? new Vector2Int(-offset.y, offset.x) : offset;

                minX = Mathf.Min(minX, rotatedOffset.x);
                maxX = Mathf.Max(maxX, rotatedOffset.x);
                minY = Mathf.Min(minY, rotatedOffset.y);
                maxY = Mathf.Max(maxY, rotatedOffset.y);
            }

            float offsetX = (minX + maxX) / 2f;
            float offsetY = (minY + maxY) / 2f;

            return new Vector2(offsetX, offsetY);
        }

#if UNITY_EDITOR
        public void SetValues(string typeId, string titleLid, Vector2Int[] cellOffSets, Sprite icon)
        {
            TypeId = typeId;
            TitleLid = titleLid;
            CellOffSets = cellOffSets;
            Icon = icon;
        }
#endif
    }
}