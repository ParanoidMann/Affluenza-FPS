using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	internal struct ActorCreationComponent
	{
		public Vector3 SpawnPoint;
		public Transform SpawnContainer;
		public ActorScriptableObject ActorConfig;
	}
}