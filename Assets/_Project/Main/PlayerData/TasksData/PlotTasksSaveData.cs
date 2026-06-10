using System;
using System.Collections.Generic;

namespace _Project.Main.PlayerData.TasksData
{
    [Serializable]
    public class PlotTasksSaveData
    {
        public List<TaskSaveData> mainTasksSaveData;
        public List<TaskSaveData> secondaryTasksSaveData;
    }
}