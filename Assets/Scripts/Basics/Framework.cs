using System;

namespace Base
{
    public class Framework
    {
        public InputManager InputManager;
        public ResourceManager ResourceManager;
        public TaskManager TaskManager;
        public PopupManager PopupManager;

        private static Framework instance;
        public static Framework Instance
        {
            get
            {
                if (instance == null) instance = new Framework();
                return instance;
            }
        }

        public Framework()
        {
            TaskManager = new TaskManager();
            ResourceManager = ResourceManager.Instance;
            InputManager = InputManager.Instance;
            PopupManager = PopupManager.Instance;
        }

        public void Init()
        {
            PopupManager.Init();

            TaskManager.AddTask(TaskEnum.RESOURCE_UPDATE, ResourceManager.RESOURCE_UPDATE_INTERVAL, -1, ResourceManager.Tick);
        }

        public void Dispose()
        {
        }
    }
}

