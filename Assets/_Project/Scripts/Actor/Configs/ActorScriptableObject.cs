using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	public abstract class ActorScriptableObject : ScriptableObject
	{
		[SerializeField]
		private ActorView _actorView;

		internal abstract ActorType ActorType { get; }

		internal ActorView ActorView => _actorView;
	}
}