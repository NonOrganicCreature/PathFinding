using Leopotam.Ecs;
using PathFinding.Components;
using UnityEngine;

namespace PathFinding
{
    public class UnitMoveSystem : IEcsRunSystem
    {
        private EcsFilter<Unit, WalkPath, Position, Rotation, UnitViewRef, UnitLocalTransform>.Exclude<UnitInAttack> _filter;
        private Configuration _configuration;
        
        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var walkPathQueue = ref _filter.Get2(index);
                ref var unitView = ref _filter.Get5(index);
                ref var unitEntity = ref _filter.GetEntity(index);
                if (_filter.GetEntity(index).IsAlive() && walkPathQueue.Value.Count > 0)
                {
                    ref var position = ref _filter.Get3(index);
                    ref var localTransform = ref _filter.Get6(index);
                    
                    if (Mathf.Approximately((walkPathQueue.Value.Peek() - position.Value).magnitude, 0))
                    {
                        walkPathQueue.Value.Dequeue();
                        if (walkPathQueue.Value.Count < 1)
                        {
                            continue;
                        }
                    }

                    Vector2 queuePeekPosition = walkPathQueue.Value.Peek();
                    Vector2 newPosition = Vector2.MoveTowards(position.Value, queuePeekPosition,
                        Time.deltaTime * _configuration.unitSpeed);

                    Debug.DrawLine(
                        new Vector3(position.Value.x + .5f, 4f, position.Value.y + .5f),
                        new Vector3(walkPathQueue.Value.Peek().x + .5f, 4f, walkPathQueue.Value.Peek().y + .5f),
                        Color.red
                    );
                    
                    
                    
                    position.Value = newPosition;
                    localTransform.Value.LookAt(
                        new Vector3(
                            (newPosition.x + .5f),
                            localTransform.Value.position.y,
                            (newPosition.y + .5f)
                        )
                    );

                    unitView.Value.transform.position = new Vector3(
                        newPosition.x,
                        unitView.Value.transform.position.y,
                        newPosition.y
                    );
                }
            }    
        }
    }
}