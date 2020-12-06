using Leopotam.Ecs;
using UnityEngine;

namespace PathFinding
{
    public class DestroyUnitSystem : IEcsRunSystem
    {
        private EcsFilter<Unit, DestroyUnitEvent, UnitViewRef> _filter;
        
        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var entity = ref _filter.GetEntity(index);
                Object.Destroy(_filter.Get3(index).Value.gameObject);
                entity.Del<UnitViewRef>();
                entity.Destroy();
            }
        }
    }
}