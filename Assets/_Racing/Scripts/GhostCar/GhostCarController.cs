using System.Collections.Generic;
using UnityEngine;

namespace _Racing.Scripts.GhostCar
{
    public class GhostCarController : MonoBehaviour
    {
        private List<RecordData> _carInputDatas;
        private bool _canDrive;
        private int _currentInputDataIndex = 0;

        public void StartDrive(List<RecordData> datas)
        {
            _carInputDatas = datas;
            _canDrive = true;
        }
        
        private void Update()
        {
            if (!_canDrive) return;

            var currentData = _carInputDatas[_currentInputDataIndex];
            
            transform.position = currentData.position;
            transform.rotation = currentData.rotation;
            _currentInputDataIndex++;

            if (_currentInputDataIndex >= _carInputDatas.Count) _canDrive = false;
        }
    }
}