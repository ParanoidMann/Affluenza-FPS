using System;
using UnityEngine;
using Tayx.Graphy;

using ParanoidMann.Core;
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
			SubscribeSceneLoading();
			BindViews();
		}

		private void BindViews()
		{
			Container.Bind<TestBoxGeometry>().FromAddressable(AddressablePaths.TestBoxGeometry);
			Container.Bind<GraphyManager>().FromAddressable(AddressablePaths.GraphyManager);

			Container.Bind<Camera>().WithId(SceneNames.TestBox).FromAddressable(AddressablePaths.TestBoxCamera);
			Container.Bind<Light>().WithId(SceneNames.TestBox).FromAddressable(AddressablePaths.TestBoxLight);
		}

		protected override void OnSceneLoaded()
		{
			Container.Bind<TestBoxSystemsBinder>().AsSingle().NonLazy();
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