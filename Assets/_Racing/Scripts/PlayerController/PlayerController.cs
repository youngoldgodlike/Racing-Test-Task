using System;
using _Racing.Scripts.GhostCar;
using Ashsvp;
using UnityEngine;

namespace _Racing.Scripts.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SimcadeVehicleController _carController;
        [SerializeField] private bool _canDrive; // Сериализовано, чтобы отлаживать на тестовой сцене

        private CarInputData _carInputData; // Класс контролер для ввода через ассет InputSystem
        private CarInput _carInput;
        
        public event Action<RecordData> onChangeFrame; // Ивент, через который передаются данные записи заезда

        private void Awake()
        {
            _carInput = new CarInput();
            _carInput.Enable();
        }
        
        public void StartDrive()
        {
            _canDrive = true;
        }
        
        private void Update()
        {
            if (!_canDrive) return;

            var inputMovementDirection = _carInput.Movement.HorizontalVertical.ReadValue<Vector2>();
            var inputBrakingValue = _carInput.Movement.Braking.ReadValue<float>();
            
            CarInputData inputData = new CarInputData()
            { 
                accelerationInput =  inputMovementDirection.y,
                steerInput =  inputMovementDirection.x,
                brakeInput = inputBrakingValue
            };
            
            _carController.SetInputData(inputData);

            RecordData recordData = new RecordData()
            {
                position = transform.position,
                rotation = transform.rotation
            };
            
           onChangeFrame?.Invoke(recordData);
        }
    }
}