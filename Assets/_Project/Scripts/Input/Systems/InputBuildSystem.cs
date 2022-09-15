using Zenject;
using UnityEngine;
using Leopotam.Ecs;
using ParanoidMann.Core.PLog;

namespace ParanoidMann.Affluenza.Input
{
	internal class InputBuildSystem :
			IEcsInitSystem
	{
		private readonly EcsWorld _ecsWorld;

		[Inject]
		private InputBuildSystem(EcsWorld ecsWorld)
		{
			_ecsWorld = ecsWorld;
		}

		public void Init()
		{
			EcsEntity input = _ecsWorld.NewEntity();

			ref var inputBase = ref input.Get<InputBaseComponent>();
			ref var moveInput = ref input.Get<MoveInputComponent>();
			ref var manipulatorInput = ref input.Get<RotationInputComponent>();

			inputBase.IsInputActive = false;

			if (Application.platform == RuntimePlatform.WindowsPlayer
					|| Application.platform == RuntimePlatform.LinuxPlayer
					|| Application.platform == RuntimePlatform.OSXPlayer)
			{
				ref var desktopPlatform = ref input.Get<DesktopPlatformComponent>();
				desktopPlatform.IsEditor = false;
			}
			else if (Application.platform == RuntimePlatform.WindowsEditor
					|| Application.platform == RuntimePlatform.LinuxEditor
					|| Application.platform == RuntimePlatform.OSXEditor)
			{
				ref var desktopPlatform = ref input.Get<DesktopPlatformComponent>();
				desktopPlatform.IsEditor = true;
			}
			else
			{
				PLog.Fatal($"{Application.platform} platform isn't supported");
			}
		}
	}
}