using System.Collections.Generic;
using System.Linq;
using _Racing.Scripts.GhostCar;

namespace _Racing.Scripts.RecordSystem
{
    public class RecordSystem 
    {
       private readonly List<RecordData> _recordInputData = new(); // Лист для хранения данных ввода о прошлом заезде
       private bool _canRecord;

       public void StartRecord()
       {
          _recordInputData.Clear(); 
          _canRecord = true;
       }
       
        public void Record(RecordData data)
        {
            if (_canRecord) _recordInputData.Add(data);
        }

        public void StopRecord() 
        {
            _canRecord = false;
        }

        public List<RecordData> GetRecordData
            => _recordInputData.ToList();
    }
}
