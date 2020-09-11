using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using Afonsoft.SetBox.Queries.Container;

namespace Afonsoft.SetBox.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}