using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.Input
{
	internal class DesktopManipulatorInputSystem :
			IEcsRunSystem
	{
		private EcsFilter<ManipulatorInputComponent, DesktopPlatformComponent> _filter;

		public void Run()
		{
			foreach (int filterIdx in _filter)
			{
				ref EcsEntity entity = ref _filter.GetEntity(filterIdx);
				ref var manipulatorInputComponent = ref entity.Get<ManipulatorInputComponent>();
			}
		}
	}
}