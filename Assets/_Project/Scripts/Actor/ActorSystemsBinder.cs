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
		private readonly PlayerWithWeaponBuildSystem _playerWithWeaponBuildSystem;
		private readonly PlayerMoveSystem _playerMoveSystem;
		private readonly PlayerFlashlightSystem _playerFlashlightSystem;
		private readonly PlayerRotationSystem _playerRotationSystem;

		private EcsSystems _actorSystems;

		[Inject]
		private ActorSystemsBinder(
				EcsWorld world,
				PlayerBuildSystem playerBuildSystem,
				PlayerWithWeaponBuildSystem playerWithWeaponBuildSystem,
				PlayerMoveSystem playerMoveSystem,
				PlayerFlashlightSystem playerFlashlightSystem,
				PlayerRotationSystem playerRotationSystem)
		{
			_world = world;

			_playerBuildSystem = playerBuildSystem;
			_playerWithWeaponBuildSystem = playerWithWeaponBuildSystem;
			_playerMoveSystem = playerMoveSystem;
			_playerFlashlightSystem = playerFlashlightSystem;
			_playerRotationSystem = playerRotationSystem;

			Init();
		}

		private void Init()
		{
			_actorSystems = new EcsSystems(_world);
			_actorSystems
					.Add(_playerBuildSystem)
					.Add(_playerWithWeaponBuildSystem)
					.Add(_playerMoveSystem)
					.Add(_playerFlashlightSystem)
					.Add(_playerRotationSystem)
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