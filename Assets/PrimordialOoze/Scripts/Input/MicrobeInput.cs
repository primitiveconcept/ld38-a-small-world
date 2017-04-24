namespace PrimordialOoze
{
	using UnityEngine;


	public abstract class MicrobeInput : MonoBehaviour
	{
		[SerializeField]
		private bool locked = false;

		private Microbe microbe;


		#region Properties
		public Microbe Microbe
		{
			get { return this.microbe; }
		}


		public PrimaryAttack PrimaryAttack
		{
			get { return GetComponent<PrimaryAttack>(); }
		}


		public SecondaryAttack SecondaryAttack
		{
			get { return GetComponent<SecondaryAttack>(); }
		}
		#endregion


		public abstract void ProcessDirectionalInput();


		public virtual void Awake()
		{
			this.microbe = GetComponent<Microbe>();
		}


		public virtual void ProcessAttackInput()
		{
		}


		public virtual void Update()
		{
			if (this.locked)
				return;

			ProcessDirectionalInput();
			ProcessAttackInput();
		}
	}
}