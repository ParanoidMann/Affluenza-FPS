using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	internal class PlayerView : ActorView
	{
		[Header("Player")]
		[SerializeField]
		private Camera _camera;

		public Camera Camera => _camera;
	}
}