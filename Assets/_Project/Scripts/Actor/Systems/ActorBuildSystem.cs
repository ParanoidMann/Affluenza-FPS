using UnityEngine;
using Leopotam.Ecs;

namespace ParanoidMann.Affluenza.Actor
{
	internal abstract class ActorBuildSystem :
			IEcsRunSystem
	{
		protected EcsFilter<ActorCreationComponent> _creationFilter;

		protected abstract ActorType ActorType { get; }

		protected abstract void Build(EcsEntity actorEntity);

		public void Run()
		{
			foreach (int filterIdx in _creationFilter)
			{
				ref EcsEntity entity = ref _creationFilter.GetEntity(filterIdx);
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