using System;
using Autofac;

namespace AutofacIssue1150
{
    public static class Program
    {
        private const string InnerScope = "InnerScope";
        
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<ScopedComponent>()
                .As<IScopedComponent>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<AutoActivatedComponent>()
                .As<IAutoActivatedComponent>()
                .InstancePerLifetimeScope()
                .AutoActivate();

            var container = builder.Build();

            using var scope = container.BeginLifetimeScope(InnerScope);
            
            var autoActivatedComponent = scope.Resolve<IAutoActivatedComponent>();
        }
    }
    
    
    public interface IScopedComponent
    {
    }

    public class ScopedComponent : IScopedComponent
    {
    }

    public interface IAutoActivatedComponent
    {
    }

    public class AutoActivatedComponent : IAutoActivatedComponent
    {
        public AutoActivatedComponent(IScopedComponent scopedComponent)
        {
            if (scopedComponent == null)
            {
                throw new ArgumentNullException(nameof(scopedComponent));
            }
        }
    }
}
