using UnityEngine;
using UnityEngine.AI;

using Leopotam.Ecs;
using ParanoidMann.Affluenza.Input;

namespace ParanoidMann.Affluenza.Actor
{
	internal class PlayerMoveSystem :
			IEcsRunSystem
	{
		private EcsFilter<ActorBaseComponent, PlayerComponent> _playerFilter;
		private EcsFilter<InputBaseComponent, InteractionInputComponent> _moveFilter;

		public void Run()
		{
			foreach (int moveFilterIdx in _moveFilter)
			{
				ref EcsEntity inputEntity = ref _moveFilter.GetEntity(moveFilterIdx);
				ref var inputBase = ref inputEntity.Get<InputBaseComponent>();

				if (inputBase.IsInputActive)
				{
					ref var interaction = ref inputEntity.Get<InteractionInputComponent>();

					foreach (int playerFilterIdx in _playerFilter)
					{
						ref EcsEntity playerEntity = ref _playerFilter.GetEntity(playerFilterIdx);
						ref var playerBase = ref playerEntity.Get<ActorBaseComponent>();
						ref var playerInfo = ref playerEntity.Get<PlayerComponent>();

						MovePlayer(interaction, playerBase, playerEntity, playerInfo);
					}
				}
			}
		}

		private void MovePlayer(
				InteractionInputComponent interaction,
				ActorBaseComponent playerBase,
				EcsEntity playerEntity,
				PlayerComponent playerInfo)
		{
			Transform transform = playerBase.GameObject.transform;

			float runMultiplier = GetRunMultiplier(playerEntity, interaction);
			Vector3 moveDirection = interaction.MoveDirection * playerInfo.MoveSpeed * runMultiplier;

			Vector3 newPosition = transform.position
					+ transform.right * moveDirection.x
					+ transform.forward * moveDirection.z;

			if (NavMesh.SamplePosition(
						newPosition,
						out NavMeshHit hit,
						playerInfo.MaxMoveDistance,
						NavMesh.AllAreas))
			{
				transform.position = hit.position;
			}
		}

		private float GetRunMultiplier(EcsEntity playerEntity, InteractionInputComponent interaction)
		{
			if (playerEntity.Has<PlayerWithWeaponComponent>() && interaction.IsRunning)
			{
				ref var playerComponent = ref playerEntity.Get<PlayerWithWeaponComponent>();
				return playerComponent.RunSpeedMultiplier;
			}

			return 1.0f;
		}
	}
}