using System;
using Zenject;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.Input
{
	public class InputSystemsBinder :
			IDisposable,
			ITickable
	{
		private readonly EcsWorld _world;
		private readonly InputBuildSystem _inputBuildSystem;

		private readonly DesktopMoveInputSystem _desktopMoveInputSystem;
		private readonly DesktopManipulatorInputSystem _desktopManipulatorInputSystem;

		private EcsSystems _inputSystems;

		[Inject]
		private InputSystemsBinder(
				EcsWorld world,
				InputBuildSystem inputBuildSystem,
				DesktopMoveInputSystem desktopMoveInputSystem,
				DesktopManipulatorInputSystem desktopManipulatorInputSystem)
		{
			_world = world;
			_inputBuildSystem = inputBuildSystem;

			_desktopMoveInputSystem = desktopMoveInputSystem;
			_desktopManipulatorInputSystem = desktopManipulatorInputSystem;

			Init();
		}

		private void Init()
		{
			_inputSystems = new EcsSystems(_world);
			_inputSystems
					.Add(_inputBuildSystem)
					.Add(_desktopMoveInputSystem)
					.Add(_desktopManipulatorInputSystem)
					.Init();
		}

		public void Tick()
		{
			_inputSystems?.Run();
		}

		public void Dispose()
		{
			_inputSystems?.Destroy();
		}
	}
}