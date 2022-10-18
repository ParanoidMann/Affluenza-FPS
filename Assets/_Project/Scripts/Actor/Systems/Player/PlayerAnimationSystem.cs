using UnityEngine;
using Leopotam.Ecs;
using ParanoidMann.Affluenza.Input;

namespace ParanoidMann.Affluenza.Actor
{
	internal class PlayerAnimationSystem :
			IEcsRunSystem
	{
		private static readonly int SpeedAnimatorHash = Animator.StringToHash("Speed");
		private static readonly int StrafeAnimatorHash = Animator.StringToHash("Strafe");

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

						MovePlayer(interaction, playerBase);
					}
				}
			}
		}

		private void MovePlayer(
				InteractionInputComponent interaction,
				ActorBaseComponent playerBase)
		{
			PlayerView playerView = playerBase.GameObject as PlayerView;
			Animator animator = playerView.Animator;

			animator.SetFloat(SpeedAnimatorHash, GetMoveAnimationValue(interaction.MoveDirection.z));
			animator.SetFloat(StrafeAnimatorHash, GetMoveAnimationValue(interaction.MoveDirection.x));
		}

		private float GetMoveAnimationValue(float direction)
		{
			if (direction > 0.0f) return 0.5f;
			if (direction < 0.0f) return -0.5f;
			return 0.0f;
		}
	}
}