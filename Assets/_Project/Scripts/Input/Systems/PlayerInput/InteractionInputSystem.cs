using Leopotam.Ecs;
using ParanoidMann.Core;

namespace ParanoidMann.Affluenza.Input
{
	internal class InteractionInputSystem :
			IEcsRunSystem
	{
		private EcsFilter<InputBaseComponent> _filter;

		public void Run()
		{
			foreach (int filterIdx in _filter)
			{
				ref EcsEntity entity = ref _filter.GetEntity(filterIdx);
				ref var interactionInputComponent = ref entity.Get<InteractionInputComponent>();

				if (UnityEngine.Input.GetButtonDown("Flashlight"))
				{
					interactionInputComponent.IsFlashlightActive.Flip();
				}
			}
		}
	}
}