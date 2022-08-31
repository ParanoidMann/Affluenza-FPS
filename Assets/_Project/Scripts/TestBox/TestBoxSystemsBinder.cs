using System;
using Zenject;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.TestBox
{
	internal class TestBoxSystemsBinder :
			IDisposable,
			ITickable
	{
		private readonly EcsSystems _testBoxSystems;

		[Inject]
		private TestBoxSystemsBinder(
				EcsWorld world,
				TestBoxBuildSystem buildSystem)
		{
			_testBoxSystems = new EcsSystems(world);
			_testBoxSystems
					.Add(buildSystem)
					.Init();
		}

		public void Tick()
		{
			_testBoxSystems.Run();
		}

		public void Dispose()
		{
			_testBoxSystems.Destroy();
		}
	}
}