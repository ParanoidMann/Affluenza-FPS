using UnityEngine;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.Input
{
	internal class RotationInputSystem :
			IEcsRunSystem
	{
		private EcsFilter<RotationInputComponent> _filter;

		public void Run()
		{
			foreach (int filterIdx in _filter)
			{
				ref EcsEntity entity = ref _filter.GetEntity(filterIdx);
				ref var rotationInputComponent = ref entity.Get<RotationInputComponent>();

				float horizontalRotation = UnityEngine.Input.GetAxis("Mouse X");
				float verticalRotation = UnityEngine.Input.GetAxis("Mouse Y");

				Vector2 rotationDirection = new Vector2(horizontalRotation, verticalRotation) * Time.deltaTime;
				rotationInputComponent.RotationDirection = rotationDirection;
			}
		}
	}
}