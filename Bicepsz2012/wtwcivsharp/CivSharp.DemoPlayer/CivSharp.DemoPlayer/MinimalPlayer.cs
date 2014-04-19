using CivSharp.Common;
using System.Linq;

namespace CivSharp.DemoPlayer
{
	public class MinimalPlayer : IPlayer
	{
		private int turn;
		private WorldInfo world;
		private UnitInfo[] myUnits;
		private PlayerInfo myPlayer;
		private CityInfo[] myCities;

		public void RefreshWorldInfo( int turn, WorldInfo world )
		{
			this.turn = turn;
			this.world = world;
			// kikeresem a saját egységeimet: ezek tulajdonosa (Owner) én vagyok
			this.myUnits = this.world.Units.
				Where( unit => unit.Owner == this.PlayerName ).ToArray();
			// kikeresem a saját játékosomat is a világ leírásából
			this.myPlayer = this.world.Players
				.Where( player => player.Name == this.PlayerName ).Single();
			// kikeresem a városaimat
			this.myCities = this.world.Cities
				.Where( city => city.Owner == this.PlayerName ).ToArray();
		}
			

		public string PlayerName
		{
			get { return "Adogatós Albatrosz"; }
		}

		public MovementData OnMovement()
		{
			if( this.myUnits.Length == 0 ) // nincs egységem => nem tudok lépni
				return null;

			var unit = this.myUnits[ 0 ];  // a legelső egységemmel fogok lépni
			var cmd = new MovementData(); // ebben adom meg, hogy mit lépek
			cmd.UnitID = unit.UnitID;
			cmd.FromX = unit.PositionX; cmd.FromY = unit.PositionY; // innen lépek
			cmd.ToX = unit.PositionX + 1; cmd.ToY = unit.PositionY; // egyet balra

			return cmd;
		}

		public ResearchData OnResearch()
		{
			return new ResearchData() { WhatToResearch = "famegmunkálás" };
		}

		public BuildingData OnBuilding()
		{
			if( this.myPlayer.Money < 140 ) // nincs elég pénzem
				return null;

			if( this.myUnits.Length == 0 ) // nincs alapító egységem
				return null;

			// veszek egy várost az első egységem helyén
			// vigyázat nem ellenőrzőm, hogy itt van-e már város!
			var unit = this.myUnits[ 0 ];
			var cmd = new BuildingData();
			cmd.PositionX = unit.PositionX;
			cmd.PositionY = unit.PositionY;
			return cmd;
		}

		public TrainingData OnTraining()
		{
			if( turn < 10 ) // a 10. kör előtt nem vásárlok
				return null;

			if( this.myPlayer.Money < 50 ) // nincs elég pénzem
				return null;

			// az első városomban veszek talpast
			var city = this.myCities[ 0 ];
			var cmd = new TrainingData();
			cmd.PositionX = city.PositionX;
			cmd.PositionY = city.PositionY;
			cmd.UnitTypeName = "talpas";
			return cmd;
		}

		public void ActionResult( bool succeeded )
		{
		}

		public void GameOver( bool winner, string message )
		{
		}

		public void EnemyDestroyed( string playerName )
		{
		}

		public void UnitLost( string unitID )
		{
		}

		public void CityLost( int positionX, int positionY )
		{
		}
	}
}