﻿using System;
using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 游戏框架入口。
    /// </summary>
    public static class GameFrameworkEntry
    {
        private static readonly GameFrameworkLinkedList<GameFrameworkModule> _gameFrameworkModules = new();

        /// <summary>
        /// 所有游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (GameFrameworkModule module in _gameFrameworkModules)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理所有游戏框架模块。
        /// </summary>
        public static void Shutdown()
        {
            for (LinkedListNode<GameFrameworkModule> current = _gameFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }

            _gameFrameworkModules.Clear();
            MemoryPool.ClearAll();
            Utility.Marshal.FreeCachedHGlobal();
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架模块类型。</typeparam>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new GameFrameworkException($"You must get module by interface, but '{interfaceType.FullName}' is not.");
            }

            if (interfaceType.FullName != null && !interfaceType.FullName.StartsWith("GameFramework.", StringComparison.Ordinal))
            {
                // 在程序集外部定义的模块
                string externalModuleName = $"{interfaceType.Namespace}.{interfaceType.Name.Substring(1)}";
                Type externalModuleType = interfaceType.Assembly.GetType(externalModuleName);
                if (typeof(GameFrameworkModule).IsAssignableFrom(externalModuleType))
                {
                    return GetModule(externalModuleType) as T;
                }

                throw new GameFrameworkException($"You must get a Game Framework module, but '{interfaceType.FullName}' is not.");
            }

            string moduleName = $"{interfaceType.Namespace}.{interfaceType.Name.Substring(1)}";
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new GameFrameworkException($"Can not find Game Framework module type '{moduleName}'.");
            }

            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要获取的游戏框架模块类型。</param>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        private static GameFrameworkModule GetModule(Type moduleType)
        {
            foreach (GameFrameworkModule module in _gameFrameworkModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return CreateModule(moduleType);
        }

        /// <summary>
        /// 创建游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要创建的游戏框架模块类型。</param>
        /// <returns>要创建的游戏框架模块。</returns>
        private static GameFrameworkModule CreateModule(Type moduleType)
        {
            GameFrameworkModule module = (GameFrameworkModule)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new GameFrameworkException($"Can not create module '{moduleType.FullName}'.");
            }

            LinkedListNode<GameFrameworkModule> current = _gameFrameworkModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                _gameFrameworkModules.AddBefore(current, module);
            }
            else
            {
                _gameFrameworkModules.AddLast(module);
            }

            return module;
        }
    }
}