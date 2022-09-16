using Zenject;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.Input
{
	internal class SettingsBuildSystem :
			IEcsInitSystem
	{
		private readonly EcsWorld _ecsWorld;

		[Inject]
		private SettingsBuildSystem(EcsWorld ecsWorld)
		{
			_ecsWorld = ecsWorld;
		}

		public void Init()
		{
			EcsEntity settings = _ecsWorld.NewEntity();
			ref var settingsComponent = ref settings.Get<SettingsComponent>();

			// TODO : Load from cache
			settingsComponent.MinVerticalCameraAngle = -90.0f;
			settingsComponent.MaxVerticalCameraAngle = 70.0f;
			settingsComponent.Sensitivity = 150.0f;
			settingsComponent.InvertVertical = false;
		}
	}
}