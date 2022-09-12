using Zenject;
using Leopotam.Ecs;

using UnityEngine;
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

			SceneManager.LoadSceneAsync(SceneNames.TestBox, LoadSceneMode.Additive).completed += OnSceneLoaded;
		}

		private void OnSceneLoaded(AsyncOperation b)
		{
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.TestBox));
		}

		private void OnDestroy()
		{
			_ecsWorld.Destroy();
		}
	}
}