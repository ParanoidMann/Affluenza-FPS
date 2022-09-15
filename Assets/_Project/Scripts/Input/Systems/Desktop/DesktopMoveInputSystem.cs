using UnityEngine;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.Input
{
	internal class DesktopMoveInputSystem :
			IEcsRunSystem
	{
		private EcsFilter<MoveInputComponent, DesktopPlatformComponent> _filter;

		public void Run()
		{
			foreach (int filterIdx in _filter)
			{
				ref EcsEntity entity = ref _filter.GetEntity(filterIdx);
				ref var moveInputComponent = ref entity.Get<MoveInputComponent>();

				float horizontalMove = UnityEngine.Input.GetAxisRaw("Horizontal");
				float verticalMove = UnityEngine.Input.GetAxisRaw("Vertical");

				Vector3 moveDirection = new Vector3(horizontalMove, 0.0f, verticalMove) * Time.deltaTime;
				moveInputComponent.Direction = moveDirection;
			}
		}
	}
}