﻿using Zenject;
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
		private readonly IActorBuilder _actorBuilder;
		private readonly IPrefabProvider _prefabProvider;

		[Inject]
		private TestBoxBuildSystem(
				IPrefabProvider prefabProvider,
				IActorBuilder actorBuilder)
		{
			_prefabProvider = prefabProvider;
			_actorBuilder = actorBuilder;
		}

		public void Init()
		{
			_prefabProvider.InstantiateOnScene<GraphyManager>(SceneNames.TestBox);
			_prefabProvider.InstantiateOnScene<Light>(SceneNames.TestBox, SceneNames.TestBox);

			TestBoxGeometryView testBox = BuildTestBox();
			_actorBuilder.BuildPlayer(PlayerSubType.Player, testBox.SpawnPoint.position, testBox.transform);
		}

		private TestBoxGeometryView BuildTestBox()
		{
			var testBox = _prefabProvider.InstantiateOnScene<TestBoxGeometryView>(SceneNames.TestBox);
			testBox.Surface.BuildNavMesh();

			return testBox;
		}
	}
}