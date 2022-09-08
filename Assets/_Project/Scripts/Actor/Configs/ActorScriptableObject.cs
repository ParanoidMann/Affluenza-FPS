using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	internal abstract class ActorScriptableObject : ScriptableObject
	{
		[Header("Actor")]
		[SerializeField]
		private ActorView _actorView;

		public abstract ActorType ActorType { get; }
		public abstract int ActorSubType { get; }

		public ActorView ActorView => _actorView;
	}
}