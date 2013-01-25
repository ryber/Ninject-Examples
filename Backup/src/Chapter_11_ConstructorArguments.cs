using Ninject;
using Ninject.Parameters;
using NUnit.Core;
using NUnit.Framework;

namespace NinjectExamples
{
    [TestFixture]
    public class Chapter_11_ConstructorArguments
    {
        [Test]
        public void CanSpecifyConstructorArgumentByParamName()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Gun>();

            var max = kernel.Get<Maximillian>(new ConstructorArgument("leftArm", new SpinnyBladeThing()));

            Assert.That(max.LeftArm, Is.InstanceOf<SpinnyBladeThing>());
            Assert.That(max.RightArm, Is.InstanceOf<Gun>());
        }


        [Test]
        public void CanSpecifyParamArguments()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Gun>();

            var bob = kernel.Get<Bob>(new PropertyValue("LeftStub", new SpinnyBladeThing()));

            Assert.That(bob.LeftStub, Is.InstanceOf<SpinnyBladeThing>());
            Assert.That(bob.RightStub, Is.InstanceOf<Gun>());
        }

        [Test]
        public void WillThrowActivationExceptionIfYouGetTheTextWrong()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Gun>();

            Assert.Throws<ActivationException>(
                () => kernel.Get<Bob>(new PropertyValue("NotRight", new SpinnyBladeThing())))
            ;
        }


        public class Maximillian
        {
            public readonly IWeapon LeftArm;
            public readonly IWeapon RightArm;

            public Maximillian(IWeapon leftArm, IWeapon rightArm)
            {
                LeftArm = leftArm;
                RightArm = rightArm;
            }
        }

        public class Bob
        {
            [Inject]
            public IWeapon LeftStub { get; set; }

            [Inject]
            public IWeapon RightStub { get; set; }
        }

        public interface IWeapon { }
        public class Gun : IWeapon{}
        public class SpinnyBladeThing : IWeapon {}
    }
}