namespace PrimordialOoze
{
	public class MapNucleus : MapCell,
		IInjectable
	{
		public bool CanBeInjectedBy(Microbe injector)
		{
			return true;
		}


		public void CompleteInjection(Microbe injector)
		{
			
			var parentData = Game.MicrobeMap.CurrentMicrobe.ParentMicrobeData;
			if (parentData != null)
			{
				Game.MicrobeMap.ExitCurrentMicrobe();
				Destroy(Game.Player);
			}
			/*	
			Game.MicrobeMap.SetCurrentMicrobe(this.Data);
			if (this.Data.ParentMicrobeData != null)
				Destroy(this.gameObject);
				*/
		}
	}
}