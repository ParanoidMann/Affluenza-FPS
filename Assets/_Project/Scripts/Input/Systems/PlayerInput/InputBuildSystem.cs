using Zenject;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.Input
{
	internal class InputBuildSystem :
			IEcsInitSystem
	{
		private EcsWorld _ecsWorld;

		public void Init()
		{
			EcsEntity input = _ecsWorld.NewEntity();

			ref var inputBase = ref input.Get<InputBaseComponent>();
			ref var interactionInput = ref input.Get<InteractionInputComponent>();

			inputBase.IsInputActive = false;
		}

		[Inject]
		private void Inject(EcsWorld ecsWorld)
		{
			_ecsWorld = ecsWorld;
		}
	}
}