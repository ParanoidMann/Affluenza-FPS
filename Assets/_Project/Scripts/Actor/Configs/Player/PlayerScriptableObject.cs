using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	[CreateAssetMenu(fileName = "Player", menuName = "Configs/Actor/Player", order = 1)]
	internal class PlayerScriptableObject : ActorScriptableObject
	{
		[Header("Player")]
		[SerializeField]
		private PlayerSubType _subType;
		[SerializeField]
		private float _moveSpeed = 1.0f;
		[SerializeField]
		private float _maxMoveDistance = 0.3f;

		public override ActorType ActorType => ActorType.Player;
		public override int ActorSubType => (int)_subType;

		public float MoveSpeed => _moveSpeed;
		public float MaxMoveDistance => _maxMoveDistance;
	}
}