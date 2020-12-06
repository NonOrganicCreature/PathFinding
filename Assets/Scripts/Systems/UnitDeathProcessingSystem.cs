using Leopotam.Ecs;
using PathFinding.Components;
using UnityEngine;

namespace PathFinding
{
    public class UnitDeathProcessingSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter<Unit, UnitTarget, UnitHealth, UnitViewRef> _filter;
        
        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var targetEntity = ref _filter.Get2(index);
                if (_filter.GetEntity(index).IsAlive() && targetEntity.Value.IsAlive())
                {
                    ref var unitHealth = ref targetEntity.Value.Get<UnitHealth>().Value;
                    if (unitHealth <= 0)
                    {
                        targetEntity.Value.Get<DestroyUnitEvent>();
                    }
                }
            }    
        }
    }
}