using Leopotam.Ecs;
using PathFinding.Components;
using UnityEngine;

namespace PathFinding
{
    public class SpawnUnitSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private GameState _gameState;
        private Configuration _configuration;
        
        public void Run()
        {
            if (_gameState.TimeUntilSpawnLeftSide <= 0)
            {
                var newUnit = _world.NewEntity();
                var unitType = Random.Range(0, 2) == 1 ? UnitType.Melee : UnitType.Range;
                newUnit.Get<Unit>().Value = unitType;
                
                var randomPosition = GetRandomPosition(UnitSide.Left);
                newUnit.Get<UnitHealth>();
                newUnit.Get<Rotation>();

                var unitView = Object.Instantiate(
                    unitType == UnitType.Melee ? _configuration.meleeView : _configuration.rangeView
                );

                unitView.transform.position = new Vector3(
                    randomPosition.x,
                    unitView.transform.position.y,
                    randomPosition.z
                );
                
                newUnit.Get<Position>().Value = new Vector2(unitView.transform.position.x, unitView.transform.position.z);
                newUnit.Get<UnitViewRef>().Value = unitView;
                unitView.EntityRef = newUnit;

                newUnit.Get<UnitSideType>().Value = UnitSide.Left;
                
                newUnit.Get<ChangeWalkPathEvent>();
                newUnit.Get<UnitHealth>().Value = _configuration.unitHealth;
                newUnit.Get<UnitAttackCoolDown>().Value = _configuration.unitAttackCooldown;
                
                MeshRenderer meshRenderer = unitView.GetComponentInChildren<MeshRenderer>();
                meshRenderer.material = _configuration.team1Material;
                
                newUnit.Get<UnitLocalTransform>().Value = meshRenderer.gameObject.transform;
                
                _gameState.TimeUntilSpawnLeftSide = _configuration.respawnSecondsLeftSide;
            }
            else
            {
                _gameState.TimeUntilSpawnLeftSide -= Time.deltaTime;
            }
            
            if (_gameState.TimeUntilSpawnRightSide <= 0)
            {
                var newUnit = _world.NewEntity();
                var unitType = Random.Range(0, 2) == 1 ? UnitType.Melee : UnitType.Range;
                
                var randomPosition = GetRandomPosition(UnitSide.Right);
                newUnit.Get<Unit>().Value = unitType;
                newUnit.Get<UnitHealth>();
                newUnit.Get<Rotation>();

                var unitView = Object.Instantiate(
                    unitType == UnitType.Melee ? _configuration.meleeView : _configuration.rangeView
                );

                // unitView.transform.localRotation = newUnit.Get<Rotation>().Value;
                unitView.transform.position = new Vector3(
                    randomPosition.x,
                    unitView.transform.position.y,
                    randomPosition.z
                );
                newUnit.Get<Position>().Value = new Vector2(unitView.transform.position.x, unitView.transform.position.z);
                
                newUnit.Get<UnitViewRef>().Value = unitView;
                unitView.EntityRef = newUnit;
                
                newUnit.Get<UnitSideType>().Value = UnitSide.Right;

                newUnit.Get<ChangeWalkPathEvent>();
                newUnit.Get<UnitHealth>().Value = _configuration.unitHealth;
                newUnit.Get<UnitAttackCoolDown>().Value = _configuration.unitAttackCooldown;
                
                MeshRenderer meshRenderer = unitView.GetComponentInChildren<MeshRenderer>();
                meshRenderer.material = _configuration.team2Material;
                
                newUnit.Get<UnitLocalTransform>().Value = meshRenderer.gameObject.transform;
                
                _gameState.TimeUntilSpawnRightSide = _configuration.respawnSecondsRightSide;
            }
            else
            {
                _gameState.TimeUntilSpawnRightSide -= Time.deltaTime;
            }
        }

        private Vector3 GetRandomPosition(UnitSide side)
        {
            switch (side)
            {
                case UnitSide.Left:
                {
                    var randomX = Random.Range(0, _configuration.gridWidth / 2);
                    var randomY = Random.Range(0, _configuration.gridHeight);
                    return new Vector3(randomX, 0, randomY);
                }
                
                case UnitSide.Right:
                {
                    var randomX = Random.Range(_configuration.gridWidth / 2, _configuration.gridWidth );
                    var randomY = Random.Range(0, _configuration.gridHeight);
                    return new Vector3(randomX, 0, randomY);
                }
                
                default:
                    throw new System.IndexOutOfRangeException("Error while generating spawn position");
            }
        }
    }
}