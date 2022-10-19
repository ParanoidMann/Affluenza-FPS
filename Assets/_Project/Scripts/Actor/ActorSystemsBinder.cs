using System;
using Zenject;
using Leopotam.Ecs;
using ParanoidMann.Core;

namespace ParanoidMann.Affluenza.Actor
{
	internal class ActorSystemsBinder :
			IDisposable,
			ITickable
	{
		private readonly EcsSystems _systems;

		[Inject]
		private ActorSystemsBinder(EcsWorld world, DiContainer container)
		{
			_systems = new EcsSystems(world);
			_systems
					.Add(container.Create<PlayerBuildSystem>())
					.Add(container.Create<PlayerWithWeaponBuildSystem>())
					.Add(container.Create<PlayerMoveSystem>())
					.Add(container.Create<PlayerRotationSystem>())
					.Add(container.Create<PlayerAnimationSystem>())
					.Add(container.Create<PlayerFlashlightSystem>())
					.Init();
		}

		public void Tick()
		{
			_systems?.Run();
		}

		public void Dispose()
		{
			_systems?.Destroy();
		}
	}
}