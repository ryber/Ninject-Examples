using System;
using Ninject;
using Xunit;
using Xunit.Should;

namespace NinjectExamples
{
    // "https://github.com/ninject/ninject/wiki/Contextual-Binding"
    public class Chapter_07_Conditional_Bindings
    {

        [Fact]
        public void CanResolveBasedOnClassAttribute()
        {
            var kernel = new StandardKernel();

            kernel.Bind<ICar>().To<Car>().WhenClassHas<City>();
            kernel.Bind<ICar>().To<Truck>().WhenClassHas<Country>();
            kernel.Bind<ICar>().To<Dunebuggy>();


            kernel.Get<CityRoad>().Car.ShouldBeInstanceOf<Car>();
            kernel.Get<CountryRoad>().Car.ShouldBeInstanceOf<Truck>();
            kernel.Get<Beach>().Car.ShouldBeInstanceOf<Dunebuggy>();
        }



        [Fact]
        public void CanResolveBasedOnMemberAttribute()
        {
            var kernel = new StandardKernel();

            kernel.Bind<ICar>().To<Car>().WhenMemberHas<City>();
            kernel.Bind<ICar>().To<Truck>().WhenMemberHas<Country>();
            kernel.Bind<ICar>().To<Dunebuggy>();
      

            var garage = kernel.Get<Garage>();
            garage.CityCar.ShouldBeInstanceOf<Car>();
            garage.CountryCar.ShouldBeInstanceOf<Truck>();

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