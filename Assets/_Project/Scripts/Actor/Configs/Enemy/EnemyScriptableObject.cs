using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	[CreateAssetMenu(fileName = "Enemy", menuName = "Configs/Actor/Enemy", order = 200)]
	internal class EnemyScriptableObject : ActorScriptableObject
	{
		[Header("Enemy")]
		[SerializeField]
		private EnemySubType _subType;

		public override ActorType ActorType => ActorType.Enemy;
		public override int ActorSubType => (int)_subType;
	}
}