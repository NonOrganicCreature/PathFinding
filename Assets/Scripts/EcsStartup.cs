using System;
using Leopotam.Ecs;
using UnityEngine;

namespace PathFinding {
    sealed class EcsStartup : MonoBehaviour {
        EcsWorld _world;
        EcsSystems _systems;
        EcsSystems _fixedUpdateSystems;
        public Configuration Configuration;
        public SceneData SceneData;
        void Start () {
            // void can be switched to IEnumerator for support coroutines.
            
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
            _fixedUpdateSystems = new EcsSystems(_world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
#endif
            
            var gameState = new GameState();
            gameState.Initialize(Configuration);
            _systems
                // register your systems here, for example:
                .Add (new InitializeGridFieldsSystem())
                // .Add (new CreateFieldViewSystem())
                .Add (new SpawnUnitSystem())
                .Add (new ChooseTargetSystem())
                .Add (new UnitSetPathSystem())
                .Add (new UnitAttackTargetSystem())
                .Add (new UnitMoveSystem())
                .Add (new UnitDeathProcessingSystem())
                .Add(new DestroyUnitSystem()).OneFrame<DestroyUnitEvent> ()
                .Add(new BulletMoveSystem())
                // register one-frame components (order is important), for example:
                .OneFrame<ChangeWalkPathEvent> ()
                
                
                // inject service instances here (order doesn't important), for example:
                .Inject (Configuration)
                .Inject (gameState)
                .Inject(SceneData)
                .Init ();
            
            _fixedUpdateSystems
                .Add (new CheckUnitInFieldPositionSystem())
                .Inject (Configuration)
                .Inject (gameState)
                .Init ();
        }

        void Update () {
            _systems?.Run ();
        }

        private void FixedUpdate()
        {
            _fixedUpdateSystems?.Run();
        }

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                _systems = null;
                _world.Destroy ();
                _world = null;
            }
        }
    }
}