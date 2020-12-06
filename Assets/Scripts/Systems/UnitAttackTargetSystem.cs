using Leopotam.Ecs;
using PathFinding.Components;
using UnityEngine;

namespace PathFinding
{
    public class UnitAttackTargetSystem : IEcsRunSystem
    {
        private EcsFilter<Unit, Position, UnitTarget, UnitAttackCoolDown, UnitLocalTransform>.Exclude<DestroyUnitEvent> _filter;
        private EcsWorld _world;
        private Configuration _configuration;
        
        public void Run()
        {
            foreach (var index in _filter)
            {
                
                ref var entity = ref _filter.GetEntity(index);
                ref var target = ref _filter.Get3(index);

                if (entity.IsAlive() && target.Value.IsAlive())
                {
                    ref var unitType = ref _filter.Get1(index);
                    ref var position = ref _filter.Get2(index);
                    ref var localTransform = ref _filter.Get5(index);
                    ref var targetPosition = ref target.Value.Get<Position>();
                    ref var targetHealth = ref target.Value.Get<UnitHealth>();
                    ref var attackCooldown = ref _filter.Get3(index).Value.Get<UnitAttackCoolDown>();
                
                    var attackDistance = unitType.Value == UnitType.Melee
                        ? _configuration.meleeAttackRadius
                        : _configuration.rangeAttackRadius;

                    var attackDamage = unitType.Value == UnitType.Melee
                        ? _configuration.meleeDamage
                        : _configuration.rangeDamage;
                
                    attackCooldown.Value -= Time.deltaTime;
                
                    if (Vector2.Distance(position.Value, targetPosition.Value) < attackDistance)
                    {
                        entity.Get<UnitInAttack>();
                        
                        localTransform.Value.LookAt(
                            new Vector3(
                                targetPosition.Value.x,
                                localTransform.Value.position.y,
                                targetPosition.Value.y 
                            )
                        );
                        
                        if (attackCooldown.Value <= 0)
                        {
                            if (unitType.Value == UnitType.Range)
                            {
                                var bulletEntity = _world.NewEntity();
                                bulletEntity.Get<Bullet>();
                                bulletEntity.Get<BulletAcceptor>().Value = target.Value;
                                bulletEntity.Get<Position>().Value = _filter.Get2(index).Value;
                                bulletEntity.Get<BulletOwner>().Value = _filter.GetEntity(index);
                                var bulletView = Object.Instantiate(_configuration.bulletView);
                                bulletEntity.Get<BulletViewRef>().Value = bulletView;
                            }
                            else
                            {
                                targetHealth.Value -= attackDamage;    
                            }
                            attackCooldown.Value = _configuration.unitAttackCooldown;
                        }
                    }
                    else
                    {
                        entity.Del<UnitInAttack>();
                    }
                }
            }
        }
    }
}