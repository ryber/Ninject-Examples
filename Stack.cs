using System;
using Ninject;
using NUnit.Framework;

namespace NinjectExamples
{
    public interface IFactory<T>
    {
        T Create();
    }

    public sealed class Presenter
    {
        private readonly View view;

        public Presenter(View view)
        {
            this.view = view;
        }
    }

    public sealed class View
    {
        public View()
        {
        }
    }

    public sealed class GoodPresenterFactory : IFactory<Presenter>
    {
        private readonly IFactory<View> viewFactory;

        public GoodPresenterFactory(IFactory<View> viewFactory)
        {
            this.viewFactory = viewFactory;
        }

        public Presenter Create()
        {
            return new Presenter(this.viewFactory.Create());
        }
    }

    public sealed class ViewFactory : IFactory<View>
    {
        public ViewFactory()
        {
        }

        public View Create()
        {
            return new View();
        }
    }

    public abstract class FactoryBase<T, U> : IFactory<T>
    {
        protected readonly U dependency;

        protected FactoryBase(U dependency)
        {
            this.dependency = dependency;
        }

        public abstract T Create();
    }

    public sealed class BadPresenterFactory : FactoryBase<Presenter, IFactory<View>>
    {
        public BadPresenterFactory(IFactory<View> viewFactory)
            : base(viewFactory)
        {
        }

        public override Presenter Create()
        {
            return new Presenter(this.dependency.Create());
        }
    }

    [TestFixture]
    public class Foo
    {
        [Test]
        public void Bar()
        {
            var container = new StandardKernel();
            //container.Bind<IFactory<Presenter>>().To<GoodPresenterFactory>();
            //container.Bind<IFactory<View>>().To<ViewFactory>();

            container.Bind<IFactory<Presenter>>().To<BadPresenterFactory>();
            container.Bind<IFactory<View>>().To<ViewFactory>();

            var presenterFactory = container.Get<IFactory<Presenter>>();

            Console.Write(presenterFactory);
        }
    }
}