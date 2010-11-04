using System;
using Ninject;
using Ninject.Activation;

namespace WindsorTests
{
	public class RoboCop
	{
		[Inject]
		public ICar Automobile { get; set; }
		
		[Inject]
		public void LoadAmmo(Ammo ammo)
		{
			HasAmmo = ammo != null;
		}

		public bool HasAmmo { get; set; }
	}

	public class Ammo{}

	public class CopShop
	{
		private ICar car;
		public bool HasMurphy { get; set; }
		public bool HasAuto { get; set; }

		[Inject]
		public CopShop(RoboCop murphy, ICar auto)
		{
			HasMurphy = murphy != null;
			car = auto;
			HasAuto = auto != null;
		}

		public ICar GetCar()
		{
			return car;
		}
	}



	public class DoughnutShop
	{
		public bool HasMurphy { get; set; }
		public bool HasAuto { get; set; }

		[Inject]
		public DoughnutShop(RoboCop murphy, Car auto)
		{
			HasMurphy = murphy != null;
			HasAuto = auto != null;
		}
	}



	public class Ed209 : IBigBadRobot
	{
		private Ed209(){}

		public static Ed209 GetAEd209()
		{
			return new Ed209();
		}
	}

	public interface IBigBadRobot
	{
	}

	public class RobotFactory : IProvider
	{
		public object Create(IContext context)
		{
			return Ed209.GetAEd209();
		}

		public Type Type
		{
			get { return typeof (IBigBadRobot); }
		}
	}
}