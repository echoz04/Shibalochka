using System.Collections.Generic;
using System.Linq;
using Sources.Runtime.Gameplay.Configs.Fish;
using Sources.Runtime.Gameplay.Inventory.Item;
using UnityEditor;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs.Items
{
    [CreateAssetMenu(fileName = "Item Config", menuName = "Configs/New Item Config")]
    public class ItemConfig : ScriptableObject
    {
        public IEnumerable<Vector3> CellPointsPosition => _cellPointsPosition;

        [field: SerializeField] public string TypeId { get; private set; }
        [field: Space]

        [field: SerializeField] public Rarity Rarity { get; private set; }
        [field: Space]

        [field: SerializeField] public bool IsMutant { get; private set; }
        [field: Space]

        [field: SerializeField] public string TitleLid { get; private set; }
        [field: Space]

        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: Space]

        [field: SerializeField] public ItemCellPoint ItemCellPointPrefab { get; private set; }
        [field: Space]

        [SerializeField] private List<Vector3> _cellPointsPosition = new();

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (string.IsNullOrWhiteSpace(TypeId))
                return;

            string assetPath = AssetDatabase.GetAssetPath(this);

            if (!string.IsNullOrEmpty(assetPath))
            {
                string currentName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                if (currentName != TypeId)
                {
                    AssetDatabase.RenameAsset(assetPath, TypeId);
                    AssetDatabase.SaveAssets();
                }
            }
#endif
        }

#if UNITY_EDITOR
        public void SetCellPointsPosition(List<Vector3> positions)
        {
            _cellPointsPosition = positions.ToList();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}