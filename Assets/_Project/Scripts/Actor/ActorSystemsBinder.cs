using System;
using Zenject;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.Actor
{
	internal class ActorSystemsBinder :
			IDisposable,
			ITickable
	{
		private readonly EcsWorld _world;

		private readonly PlayerBuildSystem _playerBuildSystem;
		private readonly PlayerMoveSystem _playerMoveSystem;

		private EcsSystems _actorSystems;

		[Inject]
		private ActorSystemsBinder(
				EcsWorld world,
				PlayerBuildSystem playerBuildSystem,
				PlayerMoveSystem playerMoveSystem)
		{
			_world = world;

			_playerBuildSystem = playerBuildSystem;
			_playerMoveSystem = playerMoveSystem;

			Init();
		}

		private void Init()
		{
			_actorSystems = new EcsSystems(_world);
			_actorSystems
					.Add(_playerBuildSystem)
					.Add(_playerMoveSystem)
					.Init();
		}

		public void Tick()
		{
			_actorSystems?.Run();
		}

		public void Dispose()
		{
			_actorSystems?.Destroy();
		}
	}
}