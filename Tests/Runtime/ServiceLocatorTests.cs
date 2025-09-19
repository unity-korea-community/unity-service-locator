using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace UNKO.ServiceLocator
{
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

        class Foo2 : IFoo
        {
            int _testNumber;

            public Foo2(int number)
            {
                _testNumber = number;
            }

            public int ReturnNumber() => _testNumber;
        }

        [TearDown]
        public void TearDown()
        {
            ServiceLocator.DisposeAllServiceLocator();
            ServiceLocator.ResetTimeoutSeconds();
        }

        [Test]
        public void ServiceLocatorTestsSimplePasses()
        {
            // Arrange
            int randomNumber = Random.Range(0, 100);
            ServiceLocator.Global.RegisterService<IFoo>(new Foo(randomNumber));

            // Act
            var foo = ServiceLocator.Global.GetService<IFoo>();

            // Assert
            Assert.AreEqual(randomNumber, foo.ReturnNumber());
        }

        [Test]
        public void ServiceLocatorTestsMultipleServices()
        {
            // Arrange
            int randomNumber = Random.Range(0, 100);
            ServiceLocator.Global.RegisterService<IFoo>(new Foo(-1));
            ServiceLocator.Global.RegisterService<IFoo>(new Foo(randomNumber));

            // Act
            var foo = ServiceLocator.Global.GetService<IFoo>();

            // Assert
            Assert.AreEqual(randomNumber, foo.ReturnNumber());
        }

        [Test]
        public void ServiceLocator_UnregisterServiceAndVerify()
        {
            // Arrange
            int randomNumber = Random.Range(0, 100);
            ServiceLocator.Global.RegisterService<IFoo>(new Foo(randomNumber));

            ServiceLocator.Global.UnregisterService<IFoo>();

            // Act
            var foo = ServiceLocator.Global.GetService<IFoo>(false);

            // Assert
            Assert.IsNull(foo);
        }

        [Test]
        public void ServiceLocator_RegisterServiceAndRetrieveFromScene()
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

        [UnityTest]
        public IEnumerator ServiceLocator_GetServiceAsync()
        {
            // Arrange
            int randomNumber = Random.Range(0, 100);
            ServiceLocator.Global.RegisterService<IFoo>(new Foo(randomNumber));
            var newObject = new GameObject();
            var serviceLocatorObject = newObject.AddComponent<ServiceLocatorGameObject>();

            // Act
            var foo = serviceLocatorObject.GetServiceAsync<IFoo>();

            // Assert
            while (foo.IsCompleted == false)
            {
                yield return null;
            }

            Assert.AreEqual(randomNumber, foo.Result.ReturnNumber());
        }

        [UnityTest]
        public IEnumerator ServiceLocator_GetServiceAsync_Lazy()
        {
            // Arrange
            int randomNumber = Random.Range(0, 100);

            // Act
            var foo = ServiceLocator.Global.GetServiceAsync<IFoo>();

            yield return null;
            ServiceLocator.Global.RegisterService<IFoo>(new Foo(randomNumber));

            // Assert
            while (foo.IsCompleted == false)
            {
                yield return null;
            }

            Assert.AreEqual(randomNumber, foo.Result.ReturnNumber());
        }

        [UnityTest]
        public IEnumerator ServiceLocator_GetServiceAsync_Timeout()
        {
            // Arrange
            ServiceLocator.DefaultSetTimeoutSeconds(0.001f);

            // NOTE unity에서 아직 Assert.ThrowsAsync를 지원하지 않음 2024기준
            // Act & Assert
            //Assert.Throws<System.TimeoutException>(() =>
            //{
            //    ServiceLocator.Global.GetServiceAsync<IFoo>();
            //});

            LogAssert.Expect(LogType.Exception, $"TimeoutException: GetServiceAsync<{typeof(IFoo).Name}> Timeout");
            var task = ServiceLocator.Global.GetServiceAsync<IFoo>();
            yield return null;
            yield return null;

            // Assert
            while (task.IsCompleted == false)
            {
                yield return null;
            }

            ServiceLocator.ResetTimeoutSeconds();
        }

        [UnityTest]
        public IEnumerator ServiceLocator_Tuple_GetServiceAsync()
        {
            // Arrange
            int randomNumber = Random.Range(0, 100);
            ServiceLocator.Global.RegisterService(new Foo(randomNumber));
            ServiceLocator.Global.RegisterService(new Foo2(randomNumber));
            var newObject = new GameObject();
            var serviceLocatorObject = newObject.AddComponent<ServiceLocatorGameObject>();

            // Act
            var foo = serviceLocatorObject.GetServiceAsync<Foo, Foo2>();

            // Assert
            while (foo.IsCompleted == false)
            {
                yield return null;
            }

            Assert.AreEqual(randomNumber, foo.Result.result1.ReturnNumber());
            Assert.AreEqual(randomNumber, foo.Result.result2.ReturnNumber());
        }

        [Test]
        public void ParallelResolveFromEachLocator()
        {
            // Arrange
            int globalNumber = Random.Range(0, 100);
            int sceneNumber = Random.Range(0, 100);
            int gameObjectNumber = Random.Range(0, 100);

            ServiceLocator.Global.RegisterService(new Foo(globalNumber));
            var newObject = new GameObject();
            ServiceLocator.SceneOf(newObject).RegisterService(new Foo2(sceneNumber));

            var serviceLocatorObject = newObject.AddComponent<ServiceLocatorGameObject>();
            serviceLocatorObject.RegisterService(new Foo(gameObjectNumber));

            // Act
            var (foo1, foo2) = ServiceLocator.GetService<Foo, Foo2>(newObject);

            // Assert
            Assert.AreEqual(gameObjectNumber, foo1.ReturnNumber());
            Assert.AreEqual(sceneNumber, foo2.ReturnNumber());
        }

        [Test]
        public void CreateOnGetServiceEvent()
        {
            // Arrange
            int callCount = 0;
            List<IFoo> foos = new();

            // Act
            var serviceDisposable = ServiceLocator.Global.CreateOnGetServiceEvent<IFoo>(foo =>
            {
                callCount++;
                foos.Add(foo);
            });

            ServiceLocator.Global.RegisterService<IFoo>(new Foo(1));
            ServiceLocator.Global.RegisterService<IFoo>(new Foo(2));

            // Assert
            Assert.AreEqual(2, callCount);
            Assert.AreEqual(2, foos.Count);
            Assert.AreEqual(1, foos[0].ReturnNumber());
            Assert.AreEqual(2, foos[1].ReturnNumber());
        }

        [Test]
        public void CreateOnGetServiceEvent_IncludeAlreadyExist()
        {
            // Arrange
            int callCount = 0;
            List<IFoo> foos = new();

            ServiceLocator.Global.RegisterService<IFoo>(new Foo(1));

            // Act
            var serviceDisposable = ServiceLocator.Global.CreateOnGetServiceEventIncludeAlreadyExist<IFoo>(foo =>
            {
                callCount++;
                foos.Add(foo);
            });

            Assert.AreEqual(1, callCount);
            Assert.AreEqual(1, foos[0].ReturnNumber());

            ServiceLocator.Global.RegisterService<IFoo>(new Foo(2));

            // Assert
            Assert.AreEqual(2, callCount);
            Assert.AreEqual(2, foos.Count);
            Assert.AreEqual(1, foos[0].ReturnNumber());
            Assert.AreEqual(2, foos[1].ReturnNumber());
        }

        [Test]
        public void CreateOnGetServiceEvent_Disposable()
        {
            // Arrange
            int callCount = 0;
            List<IFoo> foos = new();
            var serviceDisposable = ServiceLocator.Global.CreateOnGetServiceEvent<IFoo>(foo =>
            {
                callCount++;
                foos.Add(foo);
            });
            ServiceLocator.Global.RegisterService<IFoo>(new Foo(1));
            Assert.AreEqual(1, callCount);

            // Act
            serviceDisposable.Dispose();
            ServiceLocator.Global.RegisterService<IFoo>(new Foo(3));

            // Assert
            Assert.AreEqual(1, callCount);
            Assert.AreEqual(1, foos.Count);
            Assert.AreEqual(1, foos[0].ReturnNumber());
        }
    }
}