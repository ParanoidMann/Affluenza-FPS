using System;
using UnityEngine;
using Tayx.Graphy;

using ParanoidMann.Core;
using ParanoidMann.Core.PLog;
using ParanoidMann.MiniRogue.TestBox;

namespace ParanoidMann.Affluenza.TestBox
{
	public class TestBoxInstaller : SceneInstaller<TestBoxInstaller>
	{
		private static readonly Type[] Systems =
		{
				typeof(TestBoxBuildSystem)
		};

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
			Container.Bind<TestBoxGeometryView>().FromAddressable(AddressablePaths.TestBoxGeometry);
			Container.Bind<GraphyManager>().FromAddressable(AddressablePaths.GraphyManager);

			Container.Bind<Camera>().WithId(SceneNames.TestBox).FromAddressable(AddressablePaths.TestBoxCamera);
			Container.Bind<Light>().WithId(SceneNames.TestBox).FromAddressable(AddressablePaths.TestBoxLight);
		}

		protected override void OnSceneLoaded()
		{
			Container.BindInterfacesAndSelfTo<TestBoxSystemsBinder>().AsSingle().NonLazy();
			Container.Bind(Systems).AsSingle();

			Container.Resolve<TestBoxSystemsBinder>();
		}

		protected override void OnSceneUnloaded()
		{
			Container.Resolve<TestBoxSystemsBinder>().Dispose();
			Container.Unbind<TestBoxSystemsBinder>();

			foreach (Type system in Systems)
			{
				Container.Unbind(system);
			}
		}
	}
}