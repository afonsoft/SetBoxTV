using Afonsoft.SetBox.Auditing;
using Afonsoft.SetBox.Test.Base;
using Shouldly;
using Xunit;

namespace Afonsoft.SetBox.Tests.Auditing
{
    // ReSharper disable once InconsistentNaming
    public class NamespaceStripper_Tests: AppTestBase
    {
        private readonly INamespaceStripper _namespaceStripper;

        public NamespaceStripper_Tests()
        {
            _namespaceStripper = Resolve<INamespaceStripper>();
        }

        [Fact]
        public void Should_Stripe_Namespace()
        {
            var controllerName = _namespaceStripper.StripNameSpace("Afonsoft.SetBox.Web.Controllers.HomeController");
            controllerName.ShouldBe("HomeController");
        }

        [Theory]
        [InlineData("Afonsoft.SetBox.Auditing.GenericEntityService`1[[Afonsoft.SetBox.Storage.BinaryObject, Afonsoft.SetBox.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null]]", "GenericEntityService<BinaryObject>")]
        [InlineData("CompanyName.ProductName.Services.Base.EntityService`6[[CompanyName.ProductName.Entity.Book, CompanyName.ProductName.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[CompanyName.ProductName.Services.Dto.Book.CreateInput, N...", "EntityService<Book, CreateInput>")]
        [InlineData("Afonsoft.SetBox.Auditing.XEntityService`1[Afonsoft.SetBox.Auditing.AService`5[[Afonsoft.SetBox.Storage.BinaryObject, Afonsoft.SetBox.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[Afonsoft.SetBox.Storage.TestObject, Afonsoft.SetBox.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],]]", "XEntityService<AService<BinaryObject, TestObject>>")]
        public void Should_Stripe_Generic_Namespace(string serviceName, string result)
        {
            var genericServiceName = _namespaceStripper.StripNameSpace(serviceName);
            genericServiceName.ShouldBe(result);
        }
    }
}
