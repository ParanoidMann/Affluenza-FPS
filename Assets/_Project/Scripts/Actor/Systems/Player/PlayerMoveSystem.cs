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
		private EcsFilter<InputBaseComponent, MoveInputComponent> _moveFilter;

		public void Run()
		{
			foreach (int moveFilterIdx in _moveFilter)
			{
				ref EcsEntity inputEntity = ref _moveFilter.GetEntity(moveFilterIdx);
				ref var inputBase = ref inputEntity.Get<InputBaseComponent>();

				if (inputBase.IsInputActive)
				{
					ref var moveInput = ref inputEntity.Get<MoveInputComponent>();

					foreach (int playerFilterIdx in _playerFilter)
					{
						ref EcsEntity playerEntity = ref _playerFilter.GetEntity(playerFilterIdx);
						ref var playerBase = ref playerEntity.Get<ActorBaseComponent>();
						ref var playerInfo = ref playerEntity.Get<PlayerComponent>();

						MovePlayer(moveInput, ref playerBase, ref playerInfo);
					}
				}
			}
		}

		private void MovePlayer(
				MoveInputComponent moveInput,
				ref ActorBaseComponent playerBase,
				ref PlayerComponent playerInfo)
		{
			Transform transform = playerBase.GameObject.transform;
			Vector3 newPosition = transform.position + moveInput.MoveDirection * playerInfo.MoveSpeed;

			if (NavMesh.SamplePosition(
						newPosition,
						out NavMeshHit hit,
						playerInfo.MaxMoveDistance,
						NavMesh.AllAreas))
			{
				transform.position = hit.position;
			}
		}
	}
}