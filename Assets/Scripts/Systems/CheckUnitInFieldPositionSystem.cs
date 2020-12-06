using System;
using Leopotam.Ecs;
using UnityEngine;

namespace PathFinding
{
    public class CheckUnitInFieldPositionSystem : IEcsRunSystem
    {
        private EcsFilter<Unit, Position>.Exclude<ChangeWalkPathEvent, DestroyUnitEvent> _filter;
        private Configuration _configuration;
        public void Run()
        {
            foreach (var index in _filter)
            {
                var x = _filter.Get2(index).Value.x;
                var y = _filter.Get2(index).Value.y;
                if (
                    Mathf.Approximately(x, Mathf.RoundToInt(x)) &&
                    Mathf.Approximately(y, Mathf.RoundToInt(y))
                )
                {
                    _filter.GetEntity(index).Get<ChangeWalkPathEvent>();
                }
            }
        }

    }
}