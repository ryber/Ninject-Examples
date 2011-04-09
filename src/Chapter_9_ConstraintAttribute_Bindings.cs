using System;
using Ninject;
using Ninject.Planning.Bindings;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
    [TestFixture, Documentation("https://github.com/ninject/ninject/wiki/Contextual-Binding")]
    public class Chapter_9_ConstraintAttribute_Bindings
    {
        [Test]
        public void CanUseContraintAttributes()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWarrior>().To<Ninja>();
            kernel.Bind<IWarrior>().To<Samurai>().WithMetadata("CanSwim", false);
            kernel.Bind<IWarrior>().To<SpecialNinja>().WithMetadata("CanSwim", true);

            Assert.That(kernel.Get<AmphibiousAttack>().Warrior, Is.InstanceOfType(typeof(SpecialNinja)));
        }

        [Test, ExpectedException(typeof(ActivationException))]
        public void AmbiguousMappingWillThrowException()
        {
            var kernel = new StandardKernel();        
            kernel.Bind<IWarrior>().To<Samurai>().WithMetadata("CanSwim", false);
            kernel.Bind<IWarrior>().To<SpecialNinja>().WithMetadata("CanSwim", true);


            Assert.That(kernel.Get<AmbiguiousAttack>().Warrior, Is.InstanceOfType(typeof(Samurai)));
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