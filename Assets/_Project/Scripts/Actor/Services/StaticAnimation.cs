using ParanoidMann.Core;

using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	[RequireComponent(typeof(Animator))]
	public class StaticAnimation : MonoBehaviour
	{
		[SerializeField]
		private Animator _animator;
		[SerializeField]
		private string _clipTrigger;

		private void Start()
		{
			if (!_clipTrigger.IsNullOrEmptyOrWhiteSpace())
			{
				int hash = Animator.StringToHash(_clipTrigger);
				_animator.SetTrigger(hash);
			}
		}

		private void OnValidate()
		{
			if (_animator == null)
			{
				_animator = GetComponent<Animator>();
			}
		}
	}
}