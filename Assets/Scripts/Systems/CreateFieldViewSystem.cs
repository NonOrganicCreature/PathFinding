using Leopotam.Ecs;
using PathFinding.Components;
using UnityEngine;

namespace PathFinding
{
    public class CreateFieldViewSystem : IEcsInitSystem
    {
        private EcsFilter<Field, Position>.Exclude<FieldViewRef> _filter;
        private Configuration _configuration;
        
        public void Init()
        {
            foreach (var entityIndex in _filter)
            {
                ref var fieldPosition = ref _filter.Get2(entityIndex);
                var fieldView = Object.Instantiate(_configuration.fieldView);
                fieldView.transform.position = new Vector3(fieldPosition.Value.x, 0, fieldPosition.Value.y);
                
                var fieldEntity = _filter.GetEntity(entityIndex);
                fieldEntity.Get<FieldViewRef>().Value = fieldView;
                fieldView.EntityRef = fieldEntity;
            }
        }
    }
}