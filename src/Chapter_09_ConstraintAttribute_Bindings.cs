using System;
using Ninject;
using Ninject.Planning.Bindings;
using NUnit.Core;
using NUnit.Framework;


namespace NinjectExamples
{
    //"https://github.com/ninject/ninject/wiki/Contextual-Binding"
    [TestFixture]
    public class Chapter_09_ConstraintAttribute_Bindings
    {
        [Test]
        public void CanUseContraintAttributes()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWarrior>().To<Ninja>();
            kernel.Bind<IWarrior>().To<Samurai>().WithMetadata("CanSwim", false);
            kernel.Bind<IWarrior>().To<SpecialNinja>().WithMetadata("CanSwim", true);

            Assert.That(kernel.Get<AmphibiousAttack>().Warrior, Is.InstanceOf<SpecialNinja>());
        }

        [Test]
        public void AmbiguousMappingWillThrowException()
        {
            var kernel = new StandardKernel();        
            kernel.Bind<IWarrior>().To<Samurai>().WithMetadata("CanSwim", false);
            kernel.Bind<IWarrior>().To<SpecialNinja>().WithMetadata("CanSwim", true);


            Assert.Throws<ActivationException>(
                delegate
                    {
                        var warrior = kernel.Get<AmbiguiousAttack>().Warrior;
                    });
        }

        public interface IWarrior { }
        public class Ninja : IWarrior{}

        public class Samurai : IWarrior{}
        public class SpecialNinja : IWarrior{}

        private class AmbiguiousAttack
        {
            public IWarrior Warrior;

            public AmbiguiousAttack(IWarrior warrior)
            {
                Warrior = warrior;
            }
        }

        private class AmphibiousAttack
        {
            public IWarrior Warrior;

            public AmphibiousAttack([Swimmer]IWarrior warrior)
            {
                Warrior = warrior;
            }
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,AllowMultiple = true, Inherited = true)]
        private class Swimmer : ConstraintAttribute {
            public override bool Matches(IBindingMetadata metadata)
            {
                return metadata.Has("CanSwim") && metadata.Get<bool>("CanSwim");
            }
        }
    }
}