using Leopotam.Ecs;
using PathFinding.Components;
using UnityEngine;

namespace PathFinding
{
    public class BulletMoveSystem : IEcsRunSystem
    {
        private EcsFilter<Bullet, Position, BulletAcceptor, BulletViewRef, BulletOwner> _filter;
        private Configuration _configuration;
        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var bulletAcceptor = ref _filter.Get3(index);
                ref var bulletView = ref _filter.Get4(index);
                ref var bulletEntity = ref _filter.GetEntity(index);
                ref var bulletOwner = ref _filter.Get5(index);
                if (!bulletOwner.Value.IsAlive() || !bulletAcceptor.Value.IsAlive())
                {
                    Object.Destroy(bulletView.Value.gameObject);
                    bulletEntity.Destroy();
                    continue;
                }
                if (
                    _filter.GetEntity(index).IsAlive() && 
                    bulletAcceptor.Value.IsAlive() && 
                    bulletOwner.Value.IsAlive()
                )
                {
                    ref var bulletPosition = ref _filter.Get2(index);
                    ref var unitType = ref bulletOwner.Value.Get<Unit>();
                    ref var targetLocalTransform = ref bulletAcceptor.Value.Get<UnitLocalTransform>();
                    var unitLocalVector2Position = new Vector2(targetLocalTransform.Value.transform.position.x,
                        targetLocalTransform.Value.transform.position.z);
                    
                    if (
                        Vector2.Distance(
                        bulletPosition.Value, 
                        unitLocalVector2Position) < .1f
                    )
                        
                    {
                        bulletAcceptor.Value.Get<UnitHealth>().Value -= unitType.Value == UnitType.Melee
                            ? _configuration.meleeDamage
                            : _configuration.rangeDamage;
                        Object.Destroy(bulletView.Value.gameObject);
                        bulletEntity.Destroy();
                    }
                    else
                    {
                        var nextPosition = Vector2.MoveTowards(bulletPosition.Value,
                            unitLocalVector2Position, Time.deltaTime * 30f);
                        bulletPosition.Value = nextPosition;
                        bulletView.Value.transform.position = new Vector3(nextPosition.x,
                            bulletView.Value.transform.position.y, nextPosition.y);
                    }
                }
            }
        }
    }
}