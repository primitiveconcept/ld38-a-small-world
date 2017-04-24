namespace PrimordialOoze
{
	using UnityEngine;


	public abstract class OnDeathAction : MonoBehaviour
	{
		private IDamageable damageable;


		#region Properties
		public IDamageable Damageable
		{
			get { return this.damageable; }
			set { this.damageable = value; }
		}
		#endregion


		public abstract void OnKilled();


		public virtual void Awake()
		{
			this.damageable = GetComponent<IDamageable>();
			if (damageable != null)
			{
				damageable.Killed += OnKilled;
			}
		}
	}
}