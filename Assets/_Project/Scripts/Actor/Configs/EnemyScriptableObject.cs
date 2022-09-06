using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	[CreateAssetMenu(fileName = "Enemy", menuName = "Configs/Actor/Enemy", order = 2)]
	public class EnemyScriptableObject : ActorScriptableObject
	{
		internal override ActorType ActorType => ActorType.Enemy;
	}
}