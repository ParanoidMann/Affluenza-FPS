using System;
using Zenject;
using Leopotam.Ecs;
using ParanoidMann.Core;

namespace ParanoidMann.Affluenza.Input
{
	public class InputSystemsBinder :
			IDisposable,
			ITickable
	{
		private EcsSystems _systems;

		[Inject]
		private InputSystemsBinder(EcsWorld world, DiContainer container)
		{
			_systems = new EcsSystems(world);
			_systems
					.Add(container.Create<SettingsBuildSystem>())
					.Add(container.Create<MoveInputSystem>())
					.Add(container.Create<InputBuildSystem>())
					.Add(container.Create<RotationInputSystem>())
					.Add(container.Create<InteractionInputSystem>())
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