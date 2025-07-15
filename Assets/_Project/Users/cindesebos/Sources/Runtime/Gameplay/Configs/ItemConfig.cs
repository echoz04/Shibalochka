using System.Collections.Generic;
using Sources.Runtime.Gameplay.Inventory.Item;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Fish Config", menuName = "Configs/New Item Config")]
    public class ItemConfig : ScriptableObject
    {
        public IEnumerable<Vector3> CellPointsPosition => _cellPointsPosition;

        [field: SerializeField] public string TypeId { get; private set; }
        [field: SerializeField] public string TitleLid { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public ItemCellPoint ItemCellPointPrefab { get; private set; }

        [SerializeField] private List<Vector3> _cellPointsPosition = new();
    }
}