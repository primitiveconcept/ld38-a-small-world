namespace PrimordialOoze
{
	using UnityEngine;


	public abstract class SecondaryAttack : MonoBehaviour
	{
		[SerializeField]
		private string attackName;

		private Microbe microbe;


		#region Properties
		public string AttackName
		{
			get { return this.attackName; }
		}


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