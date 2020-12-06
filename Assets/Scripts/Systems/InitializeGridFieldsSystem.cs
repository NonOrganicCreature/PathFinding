using Leopotam.Ecs;
using PathFinding.Components;
using UnityEngine;


namespace PathFinding
{
    public class InitializeGridFieldsSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private Configuration _configuration;
        private SceneData _sceneData;
        public void Init()
        {
            for (int i = 0; i < _configuration.gridHeight; i++)
            {
                for (int j = 0; j < _configuration.gridWidth; j++)
                {
                    _sceneData.tilemapView.SetTile(new Vector3Int(j, i, 0), _configuration.tileView);
                }
            }
        }
    }
}