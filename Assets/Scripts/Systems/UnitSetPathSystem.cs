using System;
using System.Collections.Generic;
using System.Linq;
using EpPathFinding.cs;
using Leopotam.Ecs;
using PathFinding.Components;
using UnityEngine;

namespace PathFinding
{
    public class UnitSetPathSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private Configuration _configuration;
        private GameState _gameState;
        private EcsFilter<Unit, UnitTarget, Position, ChangeWalkPathEvent> _filter;
        
        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var entity = ref _filter.GetEntity(index);
                if (entity.IsAlive() && _filter.Get2(index).Value.IsAlive())
                {
                    ref var currentEntityPosition = ref _filter.Get3(index);
                    ref var closestEnemyEntityPosition = ref _filter.Get2(index).Value.Get<Position>();


                    _filter.GetEntity(index).Get<WalkPath>().Value = GetPathForUnit(
                        new Vector2Int(Mathf.RoundToInt(currentEntityPosition.Value.x), Mathf.RoundToInt(currentEntityPosition.Value.y)), 
                        new Vector2Int(Mathf.RoundToInt(closestEnemyEntityPosition.Value.x), Mathf.RoundToInt(closestEnemyEntityPosition.Value.y))
                    );
                    
                }
            }
        }
        
        public Queue<Vector2> GetPathForUnit(Vector2Int from, Vector2Int to)
        {
            _gameState.SearchPathPoints.Reset(new GridPos(from.x, from.y), new GridPos(to.x, to.y));
            List<GridPos> resultPath = JumpPointFinder.FindPath(_gameState.SearchPathPoints);
            Queue<Vector2> queuePath = new Queue<Vector2>();
            foreach (var point in resultPath)
            {
                // lerp path points to recalculate path foreach tile

                if (queuePath.Count > 0)
                {
                    var currentPoint = new Vector2(point.x, point.y);
                    var lerpPointsCount = Math.Abs(currentPoint.x - queuePath.Peek().x);
                    if (lerpPointsCount > 1)
                    {
                        var startSequence = queuePath.Peek();
                        var lerpStep = 1f / (lerpPointsCount + 1);
                        for (var i = lerpStep; i < 1; i += lerpStep * _configuration.pathFindingInterpolationStep)
                        {
                            var lerpPoint = Vector2.Lerp(startSequence, currentPoint, i);
                            queuePath.Enqueue(
                                new Vector2(Mathf.RoundToInt(lerpPoint.x), Mathf.RoundToInt(lerpPoint.y))
                            );
                        }
                    }
                }
                queuePath.Enqueue(new Vector2(point.x, point.y));
            }
            return queuePath;
        }

    }
}