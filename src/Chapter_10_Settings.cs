using System;
using Ninject;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
    [TestFixture]
    public class Chapter_10_Settings
    {
        [Test]
        public void CanChangeTheInjectAttribute()
        {
            var settings = new NinjectSettings();
            Assert.That(settings.InjectAttribute, Is.EqualTo(typeof(InjectAttribute)));

            settings.InjectAttribute = typeof (Weaponize);
            var kernel = new StandardKernel(settings);

            var murphy = kernel.Get<RoboCop>();

            Assert.That(murphy.LeftHand, Is.Null);
            Assert.That(murphy.RightHand, Is.Not.Null);
        }

        [Test]
        public void CanAllowInjectionIntoNonPublicProperties()
        {
            var kernel = new StandardKernel();
            
            Assert.That(kernel.Get<RoboCop>().GetGunFromLeg(), Is.Null);

            kernel = new StandardKernel(new NinjectSettings {InjectNonPublic = true});

            Assert.That(kernel.Get<RoboCop>().GetGunFromLeg(), Is.Not.Null);
        }

        [Test, Warning(@"Activating this setting has an impact on the performance. 
                         It is recomended not to use this feature and use constructor injection instead.")]
        public void CanAllowInjectionIntoPrivateBaseMembers()
        {
            var kernel = new StandardKernel(new NinjectSettings { InjectNonPublic = true });

            Assert.That(kernel.Get<RoboCop>().GetGunPrototype(), Is.Null);

            kernel = new StandardKernel(new NinjectSettings { InjectNonPublic = true, InjectParentPrivateProperties = true });

            Assert.That(kernel.Get<RoboCop>().GetGunPrototype(), Is.Not.Null);
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