using System.Collections.Generic;
using System.Linq;
using Sources.Runtime.Gameplay.Configs;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Sources.Runtime.Gameplay.Inventory.Item
{
    public class ItemCellPoint : MonoBehaviour
    {
        public Transform Transform => transform;

        public bool IsPlaced { get; set; }
    }
}
