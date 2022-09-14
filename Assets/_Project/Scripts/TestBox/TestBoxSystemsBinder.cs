using System;
using Zenject;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.TestBox
{
	internal class TestBoxSystemsBinder :
			IDisposable,
			ITickable
	{
		private readonly EcsWorld _world;
		private readonly TestBoxBuildSystem _buildSystem;

		private EcsSystems _testBoxSystems;

		[Inject]
		private TestBoxSystemsBinder(
				EcsWorld world,
				TestBoxBuildSystem buildSystem)
		{
			_world = world;
			_buildSystem = buildSystem;

			Init();
		}

		private void Init()
		{
			_testBoxSystems = new EcsSystems(_world);
			_testBoxSystems
					.Add(_buildSystem)
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