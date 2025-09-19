using NUnit.Framework;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    public class ServiceLocatorCreateTests
    {
        interface IFoo
        {
            int Number { get; }
        }

        class Foo : IFoo
        {
            public int Number { get; }

            public Foo(int number)
            {
                Number = number;
            }
        }

        class Bar
        {
            public string NumberString { get; }
            public IFoo Foo { get; }

            public Bar(string number, IFoo foo)
            {
                NumberString = number;
                Foo = foo;
            }
        }

        [TearDown]
        public void TearDown()
        {
            ServiceLocator.DisposeAllServiceLocator();
        }

        [Test]
        public void CreateResolveTests_Global()
        {
            // arrange
            int randomNumber = Random.Range(0, 100);
            Foo fooService = new Foo(randomNumber);
            string stringService = (randomNumber + 1).ToString();

            ServiceLocator.Global.RegisterServiceAndInterfaces(fooService);
            ServiceLocator.Global.RegisterService(stringService);

            // act
            var bar = ServiceLocator.Global.CreateAndResolve<Bar>();

            // assert
            Assert.AreEqual(fooService, bar.Foo);
            Assert.AreEqual(stringService, bar.NumberString);
        }

        [Test]
        public void CreateResolveTests_GameObject()
        {
            // NOTE 요청자와 가까운 순 local(=GameObject or Scene)에서 등록한 서비스가 우선순위가 높다.
            // Foo는 local 및 global 양 쪽에 등록했으나, 테스트 케이스 속 locator가 local이므로 local이 우선순위가 높다.
            // string은 global에만 등록했으므로 global에서 가져온다.

            // arrange
            int randomNumber = Random.Range(0, 100);
            Foo fooServiceFromGameObject = new Foo(randomNumber);
            string stringServiceFromGlobal = (randomNumber + 1).ToString();

            Foo fooServiceFromGlobal = new Foo(randomNumber - 1);
            ServiceLocator.Global.RegisterServiceAndInterfaces(fooServiceFromGlobal);
            ServiceLocator.Global.RegisterService(stringServiceFromGlobal);

            var locator = new GameObject().AddComponent<ServiceLocatorGameObject>();
            locator.RegisterServiceAndInterfaces(fooServiceFromGameObject);

            // act
            var bar = locator.CreateAndResolve<Bar>();

            // assert
            Assert.AreEqual(fooServiceFromGameObject, bar.Foo);
            Assert.AreEqual(stringServiceFromGlobal, bar.NumberString);
        }

        [Test]
        public void CreateResolveTests_GameObject_With_LocalResolve()
        {
            // arrange
            int randomNumber = Random.Range(0, 100);
            Foo fooServiceFromGameObject = new Foo(randomNumber);
            Foo fooServiceFromGlobal = new Foo(randomNumber - 1);
            ServiceLocator.Global.RegisterServiceAndInterfaces(fooServiceFromGlobal);

            Foo fooServiceFromLocal = new Foo(randomNumber + 1);
            string stringServiceFromLocal = (randomNumber + 2).ToString();

            var locator = new GameObject().AddComponent<ServiceLocatorGameObject>();
            locator.RegisterServiceAndInterfaces(fooServiceFromGameObject);

            // act
            var bar = locator.CreateAndResolve<Bar>(stringServiceFromLocal, fooServiceFromLocal);

            // assert
            Assert.AreEqual(fooServiceFromLocal, bar.Foo);
            Assert.AreEqual(stringServiceFromLocal, bar.NumberString);
        }
    }
}