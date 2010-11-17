using System;
using Ninject;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
    [TestFixture]
    public class ConditionalBindingTests
    {
        private IKernel _dojo;

        [SetUp]
        public void SetUpConditionalBindings()
        {
            _dojo = new StandardKernel();
        }

        [Test]
        public void CanResolveBasedOnAttribute()
        {
            _dojo.Bind<ICar>().To<Car>().WhenClassHas<City>();
            _dojo.Bind<ICar>().To<Truck>().WhenClassHas<Country>();
            _dojo.Bind<ICar>().To<Dunebuggy>();


            Assert.That(_dojo.Get<CityRoad>().Car, Is.InstanceOfType(typeof(Car)));
            Assert.That(_dojo.Get<CountryRoad>().Car, Is.InstanceOfType(typeof(Truck)));
            Assert.That(_dojo.Get<Beach>().Car, Is.InstanceOfType(typeof(Dunebuggy)));
        }
    }

    public class City : Attribute 
    {
    }

    public class Country : Attribute
    {  
    }

    public class Dunebuggy : ICar
    {
        public Engine Engine
        {
            get; set;
        }
    }


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