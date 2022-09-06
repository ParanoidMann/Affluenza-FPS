using UnityEngine;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.Actor
{
	internal abstract class ActorBuildSystem :
			IEcsRunSystem
	{
		private EcsFilter<ActorCreationComponent> _filter;

		public abstract ActorType ActorType { get; }

		protected abstract void Build(EcsEntity actorEntity);

		public void Run()
		{
			foreach (int filterIdx in _filter)
			{
				ref EcsEntity entity = ref _filter.GetEntity(filterIdx);
				ref var creationComponent = ref entity.Get<ActorCreationComponent>();

				if (ActorType == creationComponent.ActorConfig.ActorType)
				{
					Build(entity);
					entity.Del<ActorCreationComponent>();
				}
			}
		}

		protected void CreateActorBase(EcsEntity actorEntity, ActorCreationComponent creation)
		{
			ref var baseComponent = ref actorEntity.Get<ActorBaseComponent>();

			ActorView view = Object.Instantiate(creation.ActorConfig.ActorView, creation.SpawnContainer);
			view.transform.localPosition = creation.SpawnPoint;

			baseComponent.Type = ActorType;
			baseComponent.GameObject = view;
		}
	}
}