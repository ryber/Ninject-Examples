using System;
using Ninject;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
    [TestFixture]
    public class Chapter_6_Conditional_Bindings
    {

        [Test]
        public void CanResolveBasedOnAttribute()
        {
            var kernel = new StandardKernel();

            kernel.Bind<ICar>().To<Car>().WhenClassHas<City>();
            kernel.Bind<ICar>().To<Truck>().WhenClassHas<Country>();
            kernel.Bind<ICar>().To<Dunebuggy>();


            Assert.That(kernel.Get<CityRoad>().Car, Is.InstanceOfType(typeof(Car)));
            Assert.That(kernel.Get<CountryRoad>().Car, Is.InstanceOfType(typeof(Truck)));
            Assert.That(kernel.Get<Beach>().Car, Is.InstanceOfType(typeof(Dunebuggy)));
        }
    }

    public class City : Attribute {}
    public class Country : Attribute {  }

    public interface ICar{}

    public class Car : ICar{}
    public class Truck : ICar{}
    public class Dunebuggy : ICar { }

    [City]
    public class CityRoad
    {
        public readonly ICar Car;

        public CityRoad(ICar car)
        {
            Car = car;
        }
    }

    [Country]
    public class CountryRoad
    {
        public readonly ICar Car;

        public CountryRoad(ICar car)
        {
            Car = car;
        }
    }

    public class Beach
    {
        public readonly ICar Car;

        public Beach(ICar car)
        {
            Car = car;
        }
    }
}