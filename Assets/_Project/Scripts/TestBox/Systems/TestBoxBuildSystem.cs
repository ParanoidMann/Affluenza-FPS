using Zenject;
using UnityEngine;
using Tayx.Graphy;
using Leopotam.Ecs;

using ParanoidMann.Core;
using ParanoidMann.Affluenza.Actor;
using IPrefabProvider = ParanoidMann.Core.IPrefabProvider;

namespace ParanoidMann.Affluenza.TestBox
{
	internal class TestBoxBuildSystem :
			IEcsInitSystem
	{
		private readonly EcsWorld _ecsWorld;
		private readonly IPrefabProvider _prefabProvider;
		private readonly PlayerScriptableObject _playerConfig;

		[Inject]
		private TestBoxBuildSystem(
				EcsWorld ecsWorld,
				IPrefabProvider prefabProvider,
				PlayerScriptableObject playerConfig)
		{
			_ecsWorld = ecsWorld;
			_prefabProvider = prefabProvider;
			_playerConfig = playerConfig;
		}

		public void Init()
		{
			var testBox = _prefabProvider.InstantiateOnScene<TestBoxGeometryView>(SceneNames.TestBox);
			_prefabProvider.InstantiateOnScene<GraphyManager>(SceneNames.TestBox);

			_prefabProvider.InstantiateOnScene<Camera>(SceneNames.TestBox, SceneNames.TestBox);
			_prefabProvider.InstantiateOnScene<Light>(SceneNames.TestBox, SceneNames.TestBox);

			CreatePlayer(testBox.transform);
		}

		private void CreatePlayer(Transform spawnContainer)
		{
			EcsEntity player = _ecsWorld.NewEntity();
			ref var playerCreation = ref player.Get<ActorCreationComponent>();

			playerCreation.ActorConfig = _playerConfig;
			playerCreation.SpawnContainer = spawnContainer;
			playerCreation.SpawnPoint = new Vector3(0.0f, 0.0f, 0.0f);
		}
	}
}