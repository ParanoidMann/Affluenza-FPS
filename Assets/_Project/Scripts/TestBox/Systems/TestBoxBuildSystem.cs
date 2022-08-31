using Zenject;
using UnityEngine;
using Tayx.Graphy;
using Leopotam.Ecs;

using ParanoidMann.Core;
using IPrefabProvider = ParanoidMann.Core.IPrefabProvider;

namespace ParanoidMann.Affluenza.TestBox
{
	internal class TestBoxBuildSystem :
			IEcsInitSystem
	{
		private readonly IPrefabProvider _prefabProvider;

		[Inject]
		private TestBoxBuildSystem(IPrefabProvider prefabProvider)
		{
			_prefabProvider = prefabProvider;
		}

		public void Init()
		{
			_prefabProvider.InstantiateOnScene<TestBoxGeometry>(SceneNames.TestBox);
			_prefabProvider.InstantiateOnScene<GraphyManager>(SceneNames.TestBox);

			_prefabProvider.InstantiateOnScene<Camera>(SceneNames.TestBox, SceneNames.TestBox);
			_prefabProvider.InstantiateOnScene<Light>(SceneNames.TestBox, SceneNames.TestBox);
		}
	}
}