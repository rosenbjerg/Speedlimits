﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Speedlimits
{
    /// <summary>
    /// A basic rate-limiter. Limit your 
    /// </summary>
    public class Speedlimit
    {
        private readonly ConcurrentQueue<TaskCompletionSource<bool>> _queue = new ConcurrentQueue<TaskCompletionSource<bool>>();
        private int _maximum;
        private TimeSpan _timeSpan;
        
        private volatile int _overhead;
        private readonly object _lock = new object();

        /// <summary>
        /// Creates a Speedlimit instance that will only allow a number of calls to Obey to resolve over a certain period of time
        /// </summary>
        /// <param name="maximumCalls">The maximum amount of calls</param>
        /// <param name="timeSpan">The period of time</param>
        public Speedlimit(int maximumCalls, TimeSpan timeSpan)
        {
            _maximum = maximumCalls;
            _overhead = maximumCalls;
            _timeSpan = timeSpan;
            Patrol();
        }

        private async Task Patrol()
        {
            while (true)
            {
                await Task.Delay(_timeSpan);

                var newCalls = _maximum;
                var queued = new List<TaskCompletionSource<bool>>(4);
                lock (_lock)
                {
                    while (newCalls > 0 && _queue.TryDequeue(out var tcs))
                    {
                        newCalls--;
                        queued.Add(tcs);
                    }

                    _overhead = newCalls;
                }

                foreach (var tcs in queued)
                {
                    tcs.SetResult(true);
                }
            }
        }
        
        /// <summary>
        /// Obey the speedlimit asynchronously
        /// </summary>
        public Task ObeyAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            var go = false;
            lock (_lock)
            {
                if (_overhead > 0)
                {
                    _overhead--;
                    go = true;
                }
            }
            if (go) 
                return Task.CompletedTask;
            
            _queue.Enqueue(tcs);
            return tcs.Task;
        }
        
        /// <summary>
        /// Obey the speedlimit synchronously
        /// </summary>
        public void Obey()
        {
            ObeyAsync().Wait();
        }
    }
}