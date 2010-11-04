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
}