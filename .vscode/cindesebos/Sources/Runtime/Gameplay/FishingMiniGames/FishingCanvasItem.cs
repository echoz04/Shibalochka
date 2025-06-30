using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.FishingMiniGames
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FishingCanvasItem : MonoBehaviour
    {
        [field: SerializeField] public GameObject GameObject { get; private set; }
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }

        public Transform Transform => GameObject.transform;

        private void OnValidate()
        {
            GameObject ??= gameObject;
            CanvasGroup ??= GetComponent<CanvasGroup>();
        }
    }
}