using System;
using Ninject;

namespace NinjectExamples
{
	public class Car : ICar
	{
		[Inject]
		public Engine Engine { get; set; }
	}

	public class Truck : ICar
	{
		public Engine Engine
		{
			get; set;
		}
	}

	public class Engine
	{
		public string Name = "V8";
	}

	public interface ICar
	{
		Engine Engine { get; }
	}

    public class AutoDealer
    {
        public readonly ICar[] Cars;

        public AutoDealer(ICar[] cars)
        {
            Cars = cars;
        }
    }

    public class OneCarCarage
    {
        public readonly ICar Car;

        public OneCarCarage(ICar car)
        {
            Car = car;
        }
    }
}