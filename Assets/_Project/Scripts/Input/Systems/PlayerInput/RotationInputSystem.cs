using UnityEngine;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.Input
{
	internal class RotationInputSystem :
			IEcsRunSystem
	{
		private EcsFilter<InteractionInputComponent> _filter;

		public void Run()
		{
			foreach (int filterIdx in _filter)
			{
				ref EcsEntity entity = ref _filter.GetEntity(filterIdx);
				ref var interactionComponent = ref entity.Get<InteractionInputComponent>();

				float horizontalRotation = UnityEngine.Input.GetAxis("Mouse X");
				float verticalRotation = UnityEngine.Input.GetAxis("Mouse Y");

				Vector2 rotationDirection = new Vector2(horizontalRotation, verticalRotation) * Time.deltaTime;
				interactionComponent.RotationDirection = rotationDirection;
			}
		}
	}
}