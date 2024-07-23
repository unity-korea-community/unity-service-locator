using NUnit.Framework;
using UnityEngine;
using UNKO.ServiceLocator;

public class ServiceLocatorTests
{
    interface IFoo
    {
        int ReturnNumber();
    }

    class Foo : IFoo
    {
        int _testNumber;

        public Foo(int number)
        {
            _testNumber = number;
        }

        public int ReturnNumber() => _testNumber;
    }

    [Test]
    public void ServiceLocatorTestsSimplePasses()
    {
        int randomNumber = Random.Range(0, 100);
        ServiceLocator.Global.RegisterService<IFoo>(new Foo(randomNumber));

        var foo = ServiceLocator.Global.GetService<IFoo>();

        Assert.AreEqual(randomNumber, foo.ReturnNumber());
    }

    [Test]
    public void ServiceLocatorTestsMultipleServices()
    {
        int randomNumber = Random.Range(0, 100);
        ServiceLocator.Global.RegisterService<IFoo>(new Foo(-1));
        ServiceLocator.Global.RegisterService<IFoo>(new Foo(randomNumber));

        var foo = ServiceLocator.Global.GetService<IFoo>();

        Assert.AreEqual(randomNumber, foo.ReturnNumber());
    }

    [Test]
    public void ServiceLocatorTestsUnregisterService()
    {
        // Arrange
        int randomNumber = Random.Range(0, 100);
        ServiceLocator.Global.RegisterService<IFoo>(new Foo(randomNumber));

        ServiceLocator.Global.UnregisterService<IFoo>();

        // Act
        var foo = ServiceLocator.Global.GetService<IFoo>();

        // Assert
        Assert.IsNull(foo);
    }

    [Test]
    public void ServiceLocatorTestsScenePasses()
    {
        // Arrange
        int randomNumber = Random.Range(0, 100);
        ServiceLocator.Global.RegisterService<IFoo>(new Foo(randomNumber));

        var newObject = new GameObject();
        var serviceLocatorObject = newObject.AddComponent<ServiceLocatorScene>();

        // Act
        var foo = serviceLocatorObject.GetService<IFoo>();

        // Assert
        Assert.AreEqual(randomNumber, foo.ReturnNumber());
    }

    [Test]
    public void ServiceLocatorTestsGameObjectPasses()
    {
        // Arrange
        int randomNumber = Random.Range(0, 100);
        ServiceLocator.Global.RegisterService<IFoo>(new Foo(randomNumber));

        var newObject = new GameObject();
        var serviceLocatorObject = newObject.AddComponent<ServiceLocatorGameObject>();

        // Act
        var foo = serviceLocatorObject.GetService<IFoo>();

        // Assert
        Assert.AreEqual(randomNumber, foo.ReturnNumber());
    }

    [Test]
    public void ServiceLocatorTestsGameObjectRegisterServiceAndInterfacesPasses()
    {
        // Arrange
        int randomNumber = Random.Range(0, 100);
        ServiceLocator.Global.RegisterService<IFoo>(new Foo(-1));

        var newObject = new GameObject();
        var serviceLocatorObject = newObject.AddComponent<ServiceLocatorGameObject>();
        serviceLocatorObject.RegisterServiceAndInterfaces(new Foo(randomNumber));

        // Act
        var foo = serviceLocatorObject.GetService<IFoo>();

        // Assert
        Assert.AreEqual(randomNumber, foo.ReturnNumber());
    }
}
