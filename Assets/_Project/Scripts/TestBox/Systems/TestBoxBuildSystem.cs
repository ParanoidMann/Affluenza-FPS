using Zenject;
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
		private IActorBuilder _actorBuilder;
		private IPrefabProvider _prefabProvider;

		public void Init()
		{
			_prefabProvider.InstantiateOnScene<GraphyManager>(SceneNames.TestBox);

			TestBoxGeometryView testBox = BuildTestBox();
			_actorBuilder.BuildPlayer(PlayerSubType.PlayerWithWeapon, testBox.SpawnPoint.position, testBox.transform);
		}

		private TestBoxGeometryView BuildTestBox()
		{
			var testBox = _prefabProvider.InstantiateOnScene<TestBoxGeometryView>(SceneNames.TestBox);
			testBox.Surface.BuildNavMesh();

			return testBox;
		}

		[Inject]
		private void Inject(
				IPrefabProvider prefabProvider,
				IActorBuilder actorBuilder)
		{
			_prefabProvider = prefabProvider;
			_actorBuilder = actorBuilder;
		}
	}
}