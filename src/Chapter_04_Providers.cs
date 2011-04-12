using System;
using Ninject;
using Ninject.Activation;
using Xunit;
using Xunit.Should;


namespace NinjectExamples
{

    public class Chapter_04_Providers
    {
        [Fact]
        public void ClassesMustBeConstructable()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Nunchaku>();

            Assert.Throws<ActivationException>(
                              delegate
                              {
                                  kernel.Get<IWeapon>();
                              });
            
            //Nunchaku has a private constructor and so will throw a exception
        }

        [Fact]
        public void ProvidersCanBeUsedForCustomConstruction()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().ToProvider<NunchakuFactory>();

            kernel.Get<IWeapon>().ShouldBeInstanceOf<Nunchaku>();
        }

        [Fact]
        public void CanUseAnAnonymousMethodForResolution()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().ToMethod(x=>Nunchaku.GetOne());

            kernel.Get<IWeapon>().ShouldBeInstanceOf<Nunchaku>();
        }

        private interface IWeapon { }

        private class Nunchaku : IWeapon
        {
            private Nunchaku() { }

            public static Nunchaku GetOne()
            {
                return new Nunchaku();
            }
        }

        private class NunchakuFactory : IProvider
        {
            public object Create(IContext context)
            {
                return Nunchaku.GetOne();
            }

            public Type Type
            {
                get { return typeof(Nunchaku); }
            }
        }
    }
}