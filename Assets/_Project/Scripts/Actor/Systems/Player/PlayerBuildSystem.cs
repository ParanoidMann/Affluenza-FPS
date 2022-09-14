using System;
using Leopotam.Ecs;

using ParanoidMann.Core.PLog;
using ParanoidMann.Affluenza.Input;

namespace ParanoidMann.Affluenza.Actor
{
	internal class PlayerBuildSystem : ActorBuildSystem
	{
		private EcsFilter<InputBaseComponent> _inputFilter;

		protected override ActorType ActorType => ActorType.Player;

		protected override void Build(EcsEntity actorEntity)
		{
			ref var creationComponent = ref actorEntity.Get<ActorCreationComponent>();
			if (creationComponent.ActorConfig is PlayerScriptableObject playerConfig)
			{
				CreateActorBase(actorEntity, creationComponent);
				FillPlayer(actorEntity, playerConfig);
				EnableInput();
			}
			else
			{
				Type configType = creationComponent.ActorConfig.GetType();
				PLog.Fatal($"Incorrect actor config type {configType} for actor type {ActorType}");
			}
		}

		private void FillPlayer(EcsEntity playerEntity, PlayerScriptableObject playerConfig)
		{
			ref var playerComponent = ref playerEntity.Get<PlayerComponent>();

			playerComponent.MoveSpeed = playerConfig.MoveSpeed;
			playerComponent.MaxMoveDistance = playerConfig.MaxMoveDistance;
		}

		private void EnableInput()
		{
			foreach (int filterIdx in _inputFilter)
			{
				ref EcsEntity entity = ref _inputFilter.GetEntity(filterIdx);
				ref var inputBaseComponent = ref entity.Get<InputBaseComponent>();

				inputBaseComponent.IsInputActive = true;
			}
		}
	}
}