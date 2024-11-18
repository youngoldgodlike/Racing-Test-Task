using System;
using UnityEngine;

namespace _Racing.Scripts.Gameplay
{
    public class FinishLine : MonoBehaviour
    {
        public event Action onCarFinish;
        
        private void OnTriggerEnter(Collider other)
        {
            onCarFinish?.Invoke();
        }
    }
}