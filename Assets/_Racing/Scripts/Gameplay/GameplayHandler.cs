using System.Collections;
using _Racing.Scripts.GhostCar;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Racing.Scripts.Gameplay
{
    public class GameplayHandler : MonoBehaviour
    {
        [SerializeField] private FinishLine _finishLine;
        [SerializeField] private GameObject _playerCarPrefab;
        [SerializeField] private GhostCarController _ghostControllerPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _camera;
        
        [Header("UI")]
        [SerializeField] private Button _startButton;
        [SerializeField] private TextMeshProUGUI _raceNumberText;
        [SerializeField] private TextMeshProUGUI _countdownText;

        private readonly RecordSystem.RecordSystem _recordSystem = new ();
        
        private PlayerController.PlayerController _playerController;
        private GhostCarController _ghostController;
        
        private int _raceNumber = 0; // номер заезда
        
        private void Start()
        {
            _finishLine.onCarFinish += FinishRace; // подписка на: при Достижении финиша мы завершаем текущую гонку
            _startButton.onClick.AddListener(StartRace); // подписка на старт заезда по нажатию кнопк
            _startButton.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            _finishLine.onCarFinish -= FinishRace;
            _startButton.onClick.RemoveListener(StartRace);
        }

        private void StartRace() // Начало заезда
        {
            TurnOffStartButton();
            
            _raceNumber++;
            _raceNumberText.text = $"Race number: {_raceNumber}";
            
            var playerCar = Instantiate(_playerCarPrefab, _spawnPoint.position, _spawnPoint.rotation);
            
            _playerController = playerCar.GetComponentInChildren<PlayerController.PlayerController>();
            _playerController.onChangeFrame += _recordSystem.Record;
            
            if (_raceNumber > 1)
                _ghostController = Instantiate(_ghostControllerPrefab, _spawnPoint.position, _spawnPoint.rotation);
            
            StartCoroutine(StartCounter());
        }

        private IEnumerator StartCounter()  
        {
            // Отсчет перед стартом
            
            _countdownText.gameObject.SetActive(true);
            float time = 3;
            while (time > 0)
            {
                yield return null;
                time -= Time.deltaTime;

                var second = Mathf.Round(time);
                _countdownText.text = second.ToString();
            }
            _countdownText.gameObject.SetActive(false);
            
            // Сам старт
            _playerController.StartDrive();
            
            if (_raceNumber > 1) _ghostController.StartDrive(_recordSystem.GetRecordData);
            
            _recordSystem.StartRecord();
        }

        private void FinishRace()
        {
            // Уничтожение текущих инcтанциированных префабов
            if (_playerController != null) Destroy(_playerController.gameObject); 
            if (_ghostController != null) Destroy(_ghostController.gameObject);
            
            _recordSystem.StopRecord();
            TurnOnStartButton();
        }

        private void TurnOnStartButton()
        {
            Cursor.lockState = CursorLockMode.None;
            _camera.SetActive(true);
            _startButton.gameObject.SetActive(true);
        }

        private void TurnOffStartButton()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _camera.SetActive(false);
            _startButton.gameObject.SetActive(false);
        }
    }
}