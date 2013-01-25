using System;
using Ninject;
using NUnit.Core;
using NUnit.Framework;

namespace NinjectExamples
{
    // "https://github.com/ninject/ninject/wiki/Contextual-Binding"
    [TestFixture]
    public class Chapter_07_Conditional_Bindings
    {

        [Test]
        public void CanResolveBasedOnClassAttribute()
        {
            var kernel = new StandardKernel();

            kernel.Bind<ICar>().To<Car>().WhenClassHas<City>();
            kernel.Bind<ICar>().To<Truck>().WhenClassHas<Country>();
            kernel.Bind<ICar>().To<Dunebuggy>();


            Assert.That(kernel.Get<CityRoad>().Car, Is.InstanceOf<Car>());
            Assert.That(kernel.Get<CountryRoad>().Car, Is.InstanceOf<Truck>());
            Assert.That(kernel.Get<Beach>().Car, Is.InstanceOf<Dunebuggy>());
        }



        [Test]
        public void CanResolveBasedOnMemberAttribute()
        {
            var kernel = new StandardKernel();

            kernel.Bind<ICar>().To<Car>().WhenMemberHas<City>();
            kernel.Bind<ICar>().To<Truck>().WhenMemberHas<Country>();
            kernel.Bind<ICar>().To<Dunebuggy>();
      

            var garage = kernel.Get<Garage>();
            Assert.That(garage.CityCar, Is.InstanceOf<Car>());
            Assert.That(garage.CountryCar, Is.InstanceOf<Truck>());

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

    public class Barn
    {
        [Inject, Country]
        public ICar Car { get; set; }
    }

    public class Garage
    {
        [Inject, City]
        public ICar CityCar { get; set; }

        [Inject, Country]
        public ICar CountryCar { get; set; }
    }
}