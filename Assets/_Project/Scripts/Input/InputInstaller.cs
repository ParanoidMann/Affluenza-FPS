using Zenject;

using ParanoidMann.Core;
using ParanoidMann.Core.PLog;

namespace ParanoidMann.Affluenza.Input
{
	public class InputInstaller : Installer<InputInstaller>
	{
		public override void InstallBindings()
		{
			PLog.Info($"Started binding {GetType()}");

			BindSystems();
			BindConfigs();

			Container.BindInterfacesAndSelfTo<InputSystemsBinder>().AsSingle();
			Container.Resolve<InputSystemsBinder>();

			PLog.Info($"Completed binding {GetType()}");
		}

		private void BindSystems()
		{
			Container.Bind<SettingsBuildSystem>().AsSingle();

			Container.Bind<InputBuildSystem>().AsSingle();
			Container.Bind<MoveInputSystem>().AsSingle();
			Container.Bind<RotationInputSystem>().AsSingle();
			Container.Bind<InteractionInputSystem>().AsSingle();
		}

		private void BindConfigs()
		{
			Container.Bind<GameSettingsScriptableObject>().FromAddressable(AddressablePaths.GameSettings).AsSingle();
		}
	}
}