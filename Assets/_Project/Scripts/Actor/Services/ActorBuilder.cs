using Zenject;
using UnityEngine;
using Leopotam.Ecs;
using ParanoidMann.Core.PLog;

namespace ParanoidMann.Affluenza.Actor
{
	internal class ActorBuilder :
			IActorBuilder
	{
		private readonly EcsWorld _ecsWorld;
		private readonly DiContainer _container;

		[Inject]
		private ActorBuilder(
				EcsWorld ecsWorld,
				DiContainer container)
		{
			_ecsWorld = ecsWorld;
			_container = container;
		}

		public void BuildPlayer(PlayerSubType subType, Vector3 spawnPoint, Transform spawnContainer)
		{
			var config = _container.ResolveId<PlayerScriptableObject>(subType);
			if (config != null)
			{
				BuildActor(config, spawnPoint, spawnContainer);
			}
			else
			{
				PLog.Fatal($"Failed resolve PlayerScriptableObject with SubType {subType}");
			}
		}

		public void BuildEnemy(EnemySubType subType, Vector3 spawnPoint, Transform spawnContainer)
		{
			var config = _container.ResolveId<EnemyScriptableObject>(subType);
			if (config != null)
			{
				BuildActor(config, spawnPoint, spawnContainer);
			}
			else
			{
				PLog.Fatal($"Failed resolve EnemyScriptableObject with SubType {subType}");
			}
		}

		public void BuildNpc(NpcSubType subType, Vector3 spawnPoint, Transform spawnContainer)
		{
			var config = _container.ResolveId<NpcScriptableObject>(subType);
			if (config != null)
			{
				BuildActor(config, spawnPoint, spawnContainer);
			}
			else
			{
				PLog.Fatal($"Failed resolve NpcScriptableObject with SubType {subType}");
			}
		}

		private void BuildActor(ActorScriptableObject config, Vector3 spawnPoint, Transform spawnContainer)
		{
			EcsEntity actor = _ecsWorld.NewEntity();
			ref var playerCreation = ref actor.Get<ActorCreationComponent>();

			playerCreation.ActorConfig = config;
			playerCreation.SpawnContainer = spawnContainer;
			playerCreation.SpawnPoint = spawnPoint;
		}
	}
}