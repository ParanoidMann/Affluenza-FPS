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

		private readonly SettingsBuildSystem _settingsBuildSystem;

		private readonly MoveInputSystem _moveInputSystem;
		private readonly InputBuildSystem _inputBuildSystem;
		private readonly RotationInputSystem _rotationInputSystem;
		private readonly InteractionInputSystem _interactionInputSystem;

		private EcsSystems _inputSystems;

		[Inject]
		private InputSystemsBinder(
				EcsWorld world,
				SettingsBuildSystem settingsBuildSystem,
				MoveInputSystem moveInputSystem,
				InputBuildSystem inputBuildSystem,
				RotationInputSystem rotationInputSystem,
				InteractionInputSystem interactionInputSystem)
		{
			_world = world;

			_settingsBuildSystem = settingsBuildSystem;

			_moveInputSystem = moveInputSystem;
			_inputBuildSystem = inputBuildSystem;
			_rotationInputSystem = rotationInputSystem;
			_interactionInputSystem = interactionInputSystem;

			Init();
		}

		private void Init()
		{
			_inputSystems = new EcsSystems(_world);
			_inputSystems
					.Add(_settingsBuildSystem)
					.Add(_inputBuildSystem)
					.Add(_moveInputSystem)
					.Add(_rotationInputSystem)
					.Add(_interactionInputSystem)
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