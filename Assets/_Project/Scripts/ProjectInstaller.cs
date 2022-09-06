using Zenject;
using Leopotam.Ecs;

using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

using ParanoidMann.Core;
using ParanoidMann.Affluenza.Actor;
using ParanoidMann.Affluenza.TestBox;

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
			ActorInstaller.Install(Container);
			TestBoxInstaller.Install(Container);

			SceneManager.LoadScene(SceneNames.TestBox, LoadSceneMode.Additive);
		}

		private void OnDestroy()
		{
			_ecsWorld.Destroy();
		}
	}
}