using UnityEngine;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.Input
{
	internal class MoveInputSystem :
			IEcsRunSystem
	{
		private EcsFilter<InteractionInputComponent> _filter;

		public void Run()
		{
			foreach (int filterIdx in _filter)
			{
				ref EcsEntity entity = ref _filter.GetEntity(filterIdx);
				ref var interactionComponent = ref entity.Get<InteractionInputComponent>();

				float horizontalMove = UnityEngine.Input.GetAxisRaw("Horizontal");
				float verticalMove = UnityEngine.Input.GetAxisRaw("Vertical");

				Vector3 moveDirection = new Vector3(horizontalMove, 0.0f, verticalMove) * Time.deltaTime;
				interactionComponent.MoveDirection = moveDirection;
			}
		}
	}
}