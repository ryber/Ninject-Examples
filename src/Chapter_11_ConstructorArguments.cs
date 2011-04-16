using Ninject;
using Ninject.Parameters;
using Xunit;
using Xunit.Should;

namespace NinjectExamples
{
    public class Chapter_11_ConstructorArguments
    {
        [Fact]
        public void CanSpecifyConstructorArgumentByParamName()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Gun>();

            var max = kernel.Get<Maximillian>(new ConstructorArgument("leftArm", new SpinnyBladeThing()));

            max.LeftArm.ShouldBeInstanceOf<SpinnyBladeThing>();
            max.RightArm.ShouldBeInstanceOf<Gun>();
        }


        [Fact]
        public void CanSpecifyParamArguments()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Gun>();

            var bob = kernel.Get<Bob>(new PropertyValue("LeftStub", new SpinnyBladeThing()));

            bob.LeftStub.ShouldBeInstanceOf<SpinnyBladeThing>();
            bob.RightStub.ShouldBeInstanceOf<Gun>();
        }

        [Fact]
        public void WillThrowActivationExceptionIfYouGetTheTextWrong()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Gun>();

            Assert.Throws<ActivationException>(delegate
                                  {
                                          kernel.Get<Bob>(new PropertyValue("NotRight", new SpinnyBladeThing()));
                                  })
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