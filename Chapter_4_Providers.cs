using System;
using Ninject;
using Ninject.Activation;
using NUnit.Framework;

namespace NinjectExamples
{
    [TestFixture]
    public class Chapter_4_Providers
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

            kernel.Get<IWeapon>();
        }

        public interface IWeapon { }  

        public class Nunchaku : IWeapon
        {
            private Nunchaku() { }

            public static Nunchaku GetOne()
            {
                return new Nunchaku();
            }
        }

        public class NunchakuFactory : IProvider
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