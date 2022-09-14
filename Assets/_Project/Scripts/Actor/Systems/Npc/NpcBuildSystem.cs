using System;
using Leopotam.Ecs;
using ParanoidMann.Core.PLog;

namespace ParanoidMann.Affluenza.Actor
{
	internal class NpcBuildSystem : ActorBuildSystem
	{
		protected override ActorType ActorType => ActorType.Npc;

		protected override void Build(EcsEntity actorEntity)
		{
			ref var creationComponent = ref actorEntity.Get<ActorCreationComponent>();
			if (creationComponent.ActorConfig is NpcScriptableObject npcConfig)
			{
				CreateActorBase(actorEntity, creationComponent);
			}
			else
			{
				Type configType = creationComponent.ActorConfig.GetType();
				PLog.Fatal($"Incorrect actor config type {configType} for actor type {ActorType}");
			}
		}
	}
}