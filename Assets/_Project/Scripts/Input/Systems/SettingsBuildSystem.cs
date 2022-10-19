using Zenject;
using Leopotam.Ecs;
using UnityEngine.Rendering;

namespace ParanoidMann.Affluenza.Input
{
	internal class SettingsBuildSystem :
			IEcsInitSystem
	{
		private EcsWorld _ecsWorld;
		private GameSettingsScriptableObject _gameSettingsConfig;

		public void Init()
		{
			EcsEntity settings = _ecsWorld.NewEntity();
			ref var settingsComponent = ref settings.Get<SettingsComponent>();

			GraphicsSettings.renderPipelineAsset = _gameSettingsConfig.RenderPipelineAsset;
			settingsComponent.MinVerticalCameraAngle = _gameSettingsConfig.MinVerticalCameraAngle;
			settingsComponent.MaxVerticalCameraAngle = _gameSettingsConfig.MaxVerticalCameraAngle;

			// TODO : Load from cache
			settingsComponent.Sensitivity = 150.0f;
			settingsComponent.InvertVertical = false;
		}

		[Inject]
		private void Inject(
				EcsWorld ecsWorld,
				GameSettingsScriptableObject gameSettingsConfig)
		{
			_ecsWorld = ecsWorld;
			_gameSettingsConfig = gameSettingsConfig;
		}
	}
}