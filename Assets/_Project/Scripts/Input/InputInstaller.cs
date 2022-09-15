using Zenject;
using ParanoidMann.Core.PLog;

namespace ParanoidMann.Affluenza.Input
{
	public class InputInstaller : Installer<InputInstaller>
	{
		public override void InstallBindings()
		{
			PLog.Info($"Started binding {GetType()}");

			BindSystems();

			Container.BindInterfacesAndSelfTo<InputSystemsBinder>().AsSingle();
			Container.Resolve<InputSystemsBinder>();

			PLog.Info($"Completed binding {GetType()}");
		}

		private void BindSystems()
		{
			Container.Bind<SettingsBuildSystem>().AsSingle();
			Container.Bind<InputBuildSystem>().AsSingle();

			Container.Bind<DesktopMoveInputSystem>().AsSingle();
			Container.Bind<DesktopManipulatorInputSystem>().AsSingle();
		}
	}
}