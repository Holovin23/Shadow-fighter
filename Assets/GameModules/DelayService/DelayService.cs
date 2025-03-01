using System;
using System.Collections.Generic;
using UnityEngine;

namespace TFPlay.Modules.Delays
{
    public class DelayService : IDelayService
    {
        private const float CLEANUP_TIME = 1f;

        private Dictionary<Guid, IDelay> _delays = new();

        private DelayFactory _factory;

        public DelayService(DelayFactory factory)
        {
            _factory = factory;
        }

        public void Initialize()
        {
            _factory.CreateDelay<RepeatRealTime>(CLEANUP_TIME, CleanUp, null);
        }

        public Guid Do<T>(float time, Action onEnd, Action onStop = null) where T : IDelay
        {
            var delay = _factory.CreateDelay<T>(time, onEnd, onStop);
            delay.Start();

            var guid = Guid.NewGuid();
            _delays[guid] = delay;

            return guid;
        }

        public void Stop(Guid guid, bool complete = false)
        {
            if (!_delays.ContainsKey(guid))
            {
                Debug.LogError($"You trying to stop non-existent delay {guid.ToString()}");
                return;
            }

            var delay = _delays[guid];
            if (complete)
                delay.Complete();
            else
                delay.Stop();
            _delays.Remove(guid);
        }

        public void StopAll(bool complete = false)
        {
            foreach (var pair in _delays)
            {
                if (complete)
                    pair.Value.Complete();
                else
                    pair.Value.Stop();
            }

            _delays.Clear();
        }

        public float RemainingTime(Guid guid)
        {
            if (!_delays.ContainsKey(guid))
                return 0;

            return Mathf.Clamp(_delays[guid].RemainingTime, 0f, float.MaxValue);
        }

        private void CleanUp()
        {
            var toDelete = new List<Guid>();
            foreach (var pair in _delays)
            {
                if (pair.Value.IsComplete)
                    toDelete.Add(pair.Key);
            }

            if (toDelete.Count == 0)
                return;

            foreach (var guid in toDelete)
                _delays.Remove(guid);
        }
    }
}