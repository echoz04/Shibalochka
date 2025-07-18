using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sources.Runtime.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private CharacterInput _characterInput;

        [Inject]
        private void Construct(CharacterInput characterInput)
        {
            _characterInput = characterInput;
        }

        private void Start()
        {
            _characterInput.Enable();
        }

        private void OnDestroy()
        {
            _characterInput.Disable();
        }
    }
}