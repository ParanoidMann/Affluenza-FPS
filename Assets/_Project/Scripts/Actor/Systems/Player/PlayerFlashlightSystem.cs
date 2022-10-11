using Leopotam.Ecs;
using ParanoidMann.Affluenza.Input;

namespace ParanoidMann.Affluenza.Actor
{
	internal class PlayerFlashlightSystem :
			IEcsRunSystem
	{
		private EcsFilter<ActorBaseComponent, PlayerWithWeaponComponent> _playerFilter;
		private EcsFilter<InputBaseComponent, InteractionInputComponent> _interactionFilter;

		public void Run()
		{
			foreach (int interactionFilterIdx in _interactionFilter)
			{
				ref EcsEntity interactionEntity = ref _interactionFilter.GetEntity(interactionFilterIdx);
				ref var inputBase = ref interactionEntity.Get<InputBaseComponent>();

				if (inputBase.IsInputActive)
				{
					ref var interactionInput = ref interactionEntity.Get<InteractionInputComponent>();

					foreach (int playerFilterIdx in _playerFilter)
					{
						ref EcsEntity playerEntity = ref _playerFilter.GetEntity(playerFilterIdx);
						ref var playerBase = ref playerEntity.Get<ActorBaseComponent>();

						PlayerWithWeaponView playerView = playerBase.GameObject as PlayerWithWeaponView;
						playerView.Flashlight.gameObject.SetActive(interactionInput.IsFlashlightActive);
					}
				}
			}
		}
	}
}