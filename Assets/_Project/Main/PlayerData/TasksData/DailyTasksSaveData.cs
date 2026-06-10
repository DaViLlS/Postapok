using System;
using System.Collections.Generic;

namespace _Project.Main.PlayerData.TasksData
{
    [Serializable]
    public class DailyTasksSaveData
    {
        public string resetTimeData;
        public List<TaskSaveData> tasksSavesData;
    }
}