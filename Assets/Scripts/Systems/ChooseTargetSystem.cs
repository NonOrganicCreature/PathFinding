using Leopotam.Ecs;
using PathFinding.Components;
using UnityEngine;

namespace PathFinding
{
    public class ChooseTargetSystem : IEcsRunSystem
    {
        private EcsFilter<Unit, UnitSideType, Position> _filter;
        private Configuration _configuration;
        
        public void Run()
        {
            var minIndexDist = -1;
            var minDist = float.PositiveInfinity;
            foreach (var index in _filter)
            {
                foreach (var targetUnitIndex in _filter)
                {
                    if (index != targetUnitIndex && _filter.Get2(index).Value != _filter.Get2(targetUnitIndex).Value)
                    {
                        var currentDistance = 
                            Vector2.Distance(
                                _filter.Get3(index).Value, 
                                _filter.Get3(targetUnitIndex).Value
                            );
                        
                        if (currentDistance < minDist)
                        {
                            minDist = currentDistance;
                            minIndexDist = targetUnitIndex;
                        }
                    }
                }

                if (minIndexDist < 0)
                {
                    continue;
                }
                
                ref var targetChooser = ref _filter.GetEntity(index);

                if (
                    !targetChooser.Has<UnitTarget>() || 
                    !targetChooser.Get<UnitTarget>().Value.AreEquals(_filter.GetEntity(minIndexDist))
                )
                {
                    targetChooser.Get<UnitTarget>().Value = _filter.GetEntity(minIndexDist);
                    // If new entity distance is higher than the previous, recalculate walk path
                    targetChooser.Get<ChangeWalkPathEvent>();
                }

                minDist = float.PositiveInfinity;
            }
            
        }
    }
}