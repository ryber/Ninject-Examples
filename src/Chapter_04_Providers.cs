using System;
using Ninject;
using Ninject.Activation;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
    [TestFixture]
    public class Chapter_04_Providers
    {
        [Test, ExpectedException(typeof(ActivationException))]
        public void ClassesMustBeConstructable()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Nunchaku>();

            kernel.Get<IWeapon>();
            //Nunchaku has a private constructor and so will throw a exception
        }

        [Test]
        public void ProvidersCanBeUsedForCustomConstruction()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().ToProvider<NunchakuFactory>();

            Assert.That(kernel.Get<IWeapon>(), Is.InstanceOfType(typeof(Nunchaku)));
        }

        [Test]
        public void CanUseAnAnonymousMethodForResolution()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().ToMethod(x=>Nunchaku.GetOne());

            Assert.That(kernel.Get<IWeapon>(), Is.InstanceOfType(typeof(Nunchaku)));
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