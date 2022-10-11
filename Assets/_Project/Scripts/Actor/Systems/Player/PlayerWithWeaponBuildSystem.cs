using System;
using Leopotam.Ecs;

using ParanoidMann.Core.PLog;
using ParanoidMann.Affluenza.Input;

namespace ParanoidMann.Affluenza.Actor
{
	internal class PlayerWithWeaponBuildSystem : ActorBuildSystem
	{
		private EcsFilter<InputBaseComponent> _inputFilter;

		protected override ActorType ActorType => ActorType.Player;
		protected override int ActorSubType => (int)PlayerSubType.PlayerWithWeapon;

		protected override void Build(EcsEntity actorEntity)
		{
			ref var creationComponent = ref actorEntity.Get<ActorCreationComponent>();
			if (creationComponent.ActorConfig is PlayerWithWeaponScriptableObject playerConfig)
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
			ref var playerWithWeaponComponent = ref playerEntity.Get<PlayerWithWeaponComponent>();

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