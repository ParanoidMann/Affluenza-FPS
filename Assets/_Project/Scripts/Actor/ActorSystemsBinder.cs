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
		private readonly ActorBuildSystem _playerBuildSystem;

		private EcsSystems _actorSystems;

		[Inject]
		private ActorSystemsBinder(
				EcsWorld world,
				PlayerBuildSystem playerBuildSystem)
		{
			_world = world;
			_playerBuildSystem = playerBuildSystem;

			Init();
		}

		public void Init()
		{
			_actorSystems = new EcsSystems(_world);
			_actorSystems
					.Add(_playerBuildSystem)
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