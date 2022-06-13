using System;
using UnityEngine;

namespace Modding
{
    public class ModHooks
    {
        private static ModHooks _instance;
        
        public static ModHooks Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                _instance = new ModHooks();
                return _instance;
            }
        }
        
        internal GameObject OnObjectPoolSpawn(GameObject go)
        {
            if (_ObjectPoolSpawnHook == null)
                return go;
            foreach (GameObjectHandler invocation in _ObjectPoolSpawnHook.GetInvocationList())
            {
                try
                {
                    go = invocation(go);
                }
                catch (Exception ex)
                {
                }
            }
            return go;
        }
        
        public event GameObjectHandler ObjectPoolSpawnHook
        {
            add
            {
                _ObjectPoolSpawnHook += value;
            }
            remove
            {
                _ObjectPoolSpawnHook -= value;
            }
        }

        private event GameObjectHandler _ObjectPoolSpawnHook;
    }
}
