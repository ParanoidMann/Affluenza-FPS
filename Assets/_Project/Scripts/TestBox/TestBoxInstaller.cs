using Tayx.Graphy;

using ParanoidMann.Core;
using ParanoidMann.Core.PLog;
using ParanoidMann.MiniRogue.TestBox;

namespace ParanoidMann.Affluenza.TestBox
{
	public class TestBoxInstaller : SceneInstaller<TestBoxInstaller>
	{
		protected override string SceneName => SceneNames.TestBox;

		public override void InstallBindings()
		{
			PLog.Info($"Started binding {GetType()}");

			SubscribeSceneLoading();
			BindViews();

			PLog.Info($"Completed binding {GetType()}");
		}

		private void BindViews()
		{
			Container.Bind<TestBoxGeometryView>().FromAddressable(AddressablePaths.TestBoxGeometry).AsSingle();
			Container.Bind<GraphyManager>().FromAddressable(AddressablePaths.GraphyManager).AsSingle();
		}

		protected override void OnSceneLoaded()
		{
			Container.BindInterfacesAndSelfTo<TestBoxSystemsBinder>().AsSingle();
			Container.Resolve<TestBoxSystemsBinder>();
		}

		protected override void OnSceneUnloaded()
		{
			Container.Resolve<TestBoxSystemsBinder>().Dispose();
			Container.Unbind<TestBoxSystemsBinder>();
		}
	}
}