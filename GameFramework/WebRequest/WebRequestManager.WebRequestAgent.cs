﻿using System;

namespace GameFramework
{
    internal sealed partial class WebRequestManager
    {
        /// <summary>
        /// Web 请求代理。
        /// </summary>
        private sealed class WebRequestAgent : ITaskAgent<WebRequestTask>
        {
            private readonly IWebRequestAgentHelper _helper;
            private WebRequestTask _task;
            private float _waitTime;

            public Action<WebRequestAgent> WebRequestAgentStart;
            public Action<WebRequestAgent, byte[]> WebRequestAgentSuccess;
            public Action<WebRequestAgent, string> WebRequestAgentFailure;

            /// <summary>
            /// 初始化 Web 请求代理的新实例。
            /// </summary>
            /// <param name="webRequestAgentHelper">Web 请求代理辅助器。</param>
            public WebRequestAgent(IWebRequestAgentHelper webRequestAgentHelper)
            {
                _helper = webRequestAgentHelper ?? throw new GameFrameworkException("Web request agent helper is invalid.");
                _task = null;
                _waitTime = 0f;

                WebRequestAgentStart = null;
                WebRequestAgentSuccess = null;
                WebRequestAgentFailure = null;
            }

            /// <summary>
            /// 获取 Web 请求任务。
            /// </summary>
            public WebRequestTask Task => _task;

            /// <summary>
            /// 获取已经等待时间。
            /// </summary>
            public float WaitTime => _waitTime;

            /// <summary>
            /// 初始化 Web 请求代理。
            /// </summary>
            public void Initialize()
            {
                _helper.WebRequestAgentHelperComplete += OnWebRequestAgentHelperComplete;
                _helper.WebRequestAgentHelperError += OnWebRequestAgentHelperError;
            }

            /// <summary>
            /// Web 请求代理轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (_task.Status == EWebRequestTaskStatus.Doing)
                {
                    _waitTime += realElapseSeconds;
                    if (_waitTime >= _task.Timeout)
                    {
                        WebRequestAgentHelperErrorEventArgs webRequestAgentHelperErrorEventArgs = WebRequestAgentHelperErrorEventArgs.Create("Timeout");
                        OnWebRequestAgentHelperError(this, webRequestAgentHelperErrorEventArgs);
                        MemoryPool.Release(webRequestAgentHelperErrorEventArgs);
                    }
                }
            }

            /// <summary>
            /// 关闭并清理 Web 请求代理。
            /// </summary>
            public void Shutdown()
            {
                Reset();
                _helper.WebRequestAgentHelperComplete -= OnWebRequestAgentHelperComplete;
                _helper.WebRequestAgentHelperError -= OnWebRequestAgentHelperError;
            }

            /// <summary>
            /// 开始处理 Web 请求任务。
            /// </summary>
            /// <param name="task">要处理的 Web 请求任务。</param>
            /// <returns>开始处理任务的状态。</returns>
            public EStartTaskStatus Start(WebRequestTask task)
            {
                _task = task ?? throw new GameFrameworkException("Task is invalid.");
                _task.Status = EWebRequestTaskStatus.Doing;

                WebRequestAgentStart?.Invoke(this);

                byte[] postData = _task.GetPostData();
                if (postData == null)
                {
                    _helper.Request(_task.WebRequestUri, _task.UserData);
                }
                else
                {
                    _helper.Request(_task.WebRequestUri, postData, _task.UserData);
                }

                _waitTime = 0f;
                return EStartTaskStatus.CanResume;
            }

            /// <summary>
            /// 重置 Web 请求代理。
            /// </summary>
            public void Reset()
            {
                _helper.Reset();
                _task = null;
                _waitTime = 0f;
            }

            private void OnWebRequestAgentHelperComplete(object sender, WebRequestAgentHelperCompleteEventArgs e)
            {
                _helper.Reset();
                _task.Status = EWebRequestTaskStatus.Done;

                WebRequestAgentSuccess?.Invoke(this, e.GetWebResponseBytes());

                _task.Done = true;
            }

            private void OnWebRequestAgentHelperError(object sender, WebRequestAgentHelperErrorEventArgs e)
            {
                _helper.Reset();
                _task.Status = EWebRequestTaskStatus.Error;

                WebRequestAgentFailure?.Invoke(this, e.ErrorMessage);

                _task.Done = true;
            }
        }
    }
}