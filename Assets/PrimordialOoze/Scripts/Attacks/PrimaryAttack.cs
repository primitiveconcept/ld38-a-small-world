namespace PrimordialOoze
{
	using UnityEngine;


	public abstract class PrimaryAttack : MonoBehaviour
	{
		private Microbe microbe;


		#region Properties
		public Microbe Microbe
		{
			get { return this.microbe; }
			set { this.microbe = value; }
		}
		#endregion


		public abstract void Attack(float x, float y);


		public virtual void Awake()
		{
			this.microbe = GetComponent<Microbe>();
		}
	}
}