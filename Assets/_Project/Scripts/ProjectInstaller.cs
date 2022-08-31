using Zenject;
using Leopotam.Ecs;

using ParanoidMann.Core;
using ParanoidMann.Affluenza.TestBox;

using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

namespace ParanoidMann.Affluenza
{
	public class ProjectInstaller : MonoInstaller
	{
		private EcsWorld _ecsWorld;

		public override void InstallBindings()
		{
			Addressables.InitializeAsync().WaitForCompletion();

			_ecsWorld = new EcsWorld();
			Container.Bind<EcsWorld>().FromInstance(_ecsWorld).AsSingle();

			CoreInstaller.Install(Container);
			TestBoxInstaller.Install(Container);

			SceneManager.LoadScene(SceneNames.TestBox, LoadSceneMode.Additive);
		}

		private void OnDestroy()
		{
			_ecsWorld.Destroy();
		}
	}
}