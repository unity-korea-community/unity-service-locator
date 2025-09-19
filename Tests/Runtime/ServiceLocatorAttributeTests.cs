using NUnit.Framework;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    public class ServiceLocatorAttributeTests : MonoBehaviour
    {
        [FromServiceLocator(FromWhere.Global)]
        public string FromGlobal;

        [FromServiceLocator(FromWhere.Scene)]
        private string _fromScene;

        [FromServiceLocator(FromWhere.GameObject)]
        public string FromGameObject { get; set; }

        public string FromGameObjectField => _fromScene;

        [Test]
        public void AttributeTests()
        {
            // arrange
            var gameObject = new GameObject();
            var tester = gameObject.AddComponent<ServiceLocatorAttributeTests>();
            gameObject.AddComponent<ServiceLocatorGameObject>();

            ServiceLocator.Global.RegisterService(nameof(AttributeTests) + "FromGlobalValue");
            ServiceLocator.SceneOf(tester).RegisterService(nameof(AttributeTests) + "FromSceneValue");
            ServiceLocator.GameObjectOf(tester).RegisterService(nameof(AttributeTests) + "FromGameObjectValue");

            // act
            ServiceLocator.ResolveServiceAttribute(tester);

            // assert
            Assert.AreEqual(nameof(AttributeTests) + "FromGlobalValue", tester.FromGlobal);
            Assert.AreEqual(nameof(AttributeTests) + "FromSceneValue", tester.FromGameObjectField);
            Assert.AreEqual(nameof(AttributeTests) + "FromGameObjectValue", tester.FromGameObject);
        }
    }

}