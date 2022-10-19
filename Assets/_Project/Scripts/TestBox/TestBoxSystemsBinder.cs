using System;
using Zenject;
using Leopotam.Ecs;
using ParanoidMann.Core;

namespace ParanoidMann.Affluenza.TestBox
{
	internal class TestBoxSystemsBinder :
			IDisposable,
			ITickable
	{
		private readonly EcsSystems _testBoxSystems;

		[Inject]
		private TestBoxSystemsBinder(EcsWorld world, DiContainer container)
		{
			_testBoxSystems = new EcsSystems(world);
			_testBoxSystems
					.Add(container.Create<TestBoxBuildSystem>())
					.Init();
		}

		public void Tick()
		{
			_testBoxSystems?.Run();
		}

		public void Dispose()
		{
			_testBoxSystems?.Destroy();
		}
	}
}