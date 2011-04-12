using System;
using Ninject;
using Xunit;
using Xunit.Should;


namespace NinjectExamples
{
    public class Chapter_10_Settings
    {
        [Fact]
        public void CanChangeTheInjectAttribute()
        {
            var settings = new NinjectSettings();

            settings.InjectAttribute.ShouldBe(typeof(InjectAttribute));
 

            settings.InjectAttribute = typeof (Weaponize);
            var kernel = new StandardKernel(settings);

            var murphy = kernel.Get<RoboCop>();

            murphy.LeftHand.ShouldBeNull();
            murphy.RightHand.ShouldNotBeNull();
        }

        [Fact]
        public void CanAllowInjectionIntoNonPublicProperties()
        {
            var kernel = new StandardKernel();
            
            kernel.Get<RoboCop>().GetGunFromLeg().ShouldBeNull();

            kernel = new StandardKernel(new NinjectSettings {InjectNonPublic = true});

            kernel.Get<RoboCop>().GetGunFromLeg().ShouldNotBeNull();
        }

        [Fact]
        public void CanAllowInjectionIntoPrivateBaseMembers()
        {
            var kernel = new StandardKernel(new NinjectSettings { InjectNonPublic = true });

            kernel.Get<RoboCop>().GetGunPrototype().ShouldBeNull();

            kernel = new StandardKernel(new NinjectSettings { InjectNonPublic = true, InjectParentPrivateProperties = true });

            kernel.Get<RoboCop>().GetGunPrototype().ShouldNotBeNull();
        }


        private class Gun {}
        private class RoboCop : OCPPrototype
        {
            [Inject]
            public Gun LeftHand { get; set; }

            [Weaponize]
            public Gun RightHand { get; set; }

            [Inject]
            private Gun HiddenLegCompartment { get; set; }

            public Gun GetGunFromLeg()
            {
                return HiddenLegCompartment;
            }
        }

        private abstract class OCPPrototype
        {
            [Inject]
            private Gun GunPrototype { get; set; }

            public Gun GetGunPrototype()
            {
                return GunPrototype;
            }
        }

        private class Weaponize : Attribute{}
    }
}