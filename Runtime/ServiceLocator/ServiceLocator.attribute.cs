using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    public static partial class ServiceLocator
    {
        public static void ResolveServiceAttribute(Component component)
            => ServiceLocator.ResolveServiceAttribute(component, component);

        public static void ResolveServiceAttribute(object @object, Component componentOrNull)
        {
            if (@object == null)
            {
                throw new Exception($"ServiceLocator.ResolveServiceAttribute: object is null, componentName:{componentOrNull?.name}");
            }

            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            // reflection으로 attribute를 찾아서 등록
            var type = @object.GetType();
            var fields = type.GetFields(bindingFlags);

            foreach (var field in fields)
            {
                // attribute 찾기
                var attribute = field.GetCustomAttributes(typeof(IFromServiceLocatorAttribute), true).FirstOrDefault() as IFromServiceLocatorAttribute;
                if (attribute == null)
                {
                    continue;
                }

                if (attribute.Lazyable)
                {
                    // 기존 ContinueWith 사용 부분을 async/await로 변경
                    async void SetFieldAsync(FieldInfo field, FromWhere where, Component componentOrNull, object target)
                    {
                        try
                        {
                            var result = await GetValueFromServiceAsync(where, componentOrNull, field.FieldType);
                            field.SetValue(target, result);
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError($"ServiceLocator.ResolveServiceAttribute: Exception occurred while resolving field '{field.Name}' of type '{field.FieldType.Name}' in object of type '{@object.GetType().Name}' - {ex.Message}", componentOrNull);
                            throw;
                        }
                    }

                    SetFieldAsync(field, attribute.Where, componentOrNull, @object);
                }
                else
                {
                    var value = GetValueFromService(attribute.Where, componentOrNull, field.FieldType);
                    field.SetValue(@object, value);
                }
            }

            var properties = type.GetProperties(bindingFlags);
            foreach (var property in properties)
            {
                // attribute 찾기
                var attribute = property.GetCustomAttributes(typeof(IFromServiceLocatorAttribute), true).FirstOrDefault() as IFromServiceLocatorAttribute;
                if (attribute == null)
                {
                    continue;
                }

                if (attribute.Lazyable)
                {
                    // 기존 ContinueWith 사용 부분을 async/await로 변경
                    async void SetFieldAsync(PropertyInfo property, FromWhere where, Component componentOrNull, object target)
                    {
                        try
                        {
                            var result = await GetValueFromServiceAsync(where, componentOrNull, property.PropertyType);
                            property.SetValue(target, result);
                        }
                        catch (System.Exception ex)
                        {
                            // timeout이 아니라면 접근 한정자를 확인해볼 것
                            Debug.LogError($"ServiceLocator.ResolveServiceAttribute: Exception occurred while resolving property '{property.Name}' of type '{property.PropertyType.Name}' in object of type '{@object.GetType().Name}' - {ex.Message}", componentOrNull);
                            var result = await GetValueFromServiceAsync(where, componentOrNull, property.PropertyType);
                            throw;
                        }
                    }

                    SetFieldAsync(property, attribute.Where, componentOrNull, @object);
                }
                else
                {
                    var value = GetValueFromService(attribute.Where, componentOrNull, property.PropertyType);
                    property.SetValue(@object, value);
                }
            }
        }

        public static async Task ResolveServiceAttributeAsync(object @object, Component componentOrNull)
        {
            List<Task> tasks = new List<Task>();
            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public;
            // reflection으로 attribute를 찾아서 등록
            var type = @object.GetType();
            var fields = type.GetFields(bindingFlags);
            try
            {
                foreach (var field in fields)
                {
                    // attribute 찾기
                    var attribute = field.GetCustomAttributes(typeof(IFromServiceLocatorAttribute), true).FirstOrDefault() as IFromServiceLocatorAttribute;
                    if (attribute == null)
                    {
                        continue;
                    }

                    var serviceLocator = GetServiceLocator(attribute.Where, componentOrNull);

                    // 기존 ContinueWith 사용 부분을 async/await로 변경
                    async Task SetFieldAsync()
                    {
                        var result = await serviceLocator.GetServiceAsync(field.FieldType);
                        field.SetValue(@object, result);
                    }
                    tasks.Add(SetFieldAsync());
                }

                var properties = type.GetProperties(bindingFlags);
                foreach (var property in properties)
                {
                    // attribute 찾기
                    var attribute = property.GetCustomAttributes(typeof(IFromServiceLocatorAttribute), true).FirstOrDefault() as IFromServiceLocatorAttribute;
                    if (attribute == null)
                    {
                        continue;
                    }

                    var serviceLocator = GetServiceLocator(attribute.Where, componentOrNull);

                    // 기존 ContinueWith 사용 부분을 async/await로 변경
                    async Task SetFieldAsync()
                    {
                        var result = await serviceLocator.GetServiceAsync(property.PropertyType);
                        property.SetValue(@object, result);
                    }
                    tasks.Add(SetFieldAsync());
                }

                await Task.WhenAll(tasks);
            }
            catch (System.Exception ex)
            {
                // timeout이 아니라면 접근 한정자를 확인해볼 것
                Debug.LogError($"ServiceLocator.ResolveServiceAttributeAsync: Exception occurred while resolving services for {@object.GetType().Name} - {ex.Message}", componentOrNull);
                throw;
            }
        }

        static object GetValueFromService(FromWhere where, Component componentOrNull, System.Type type)
            => GetServiceLocator(where, componentOrNull).GetService(type, true);

        static async Task<object> GetValueFromServiceAsync(FromWhere where, Component componentOrNull, System.Type type)
            => await GetServiceLocator(where, componentOrNull).GetServiceAsync(type);

        static IServiceLocator GetServiceLocator(FromWhere where, Component componentOrNull)
        {
            if (componentOrNull == null)
            {
                return Global;
            }

            switch (where)
            {
                case FromWhere.Global:
                    return Global;
                case FromWhere.Scene:
                    return SceneOf(componentOrNull);
                case FromWhere.GameObject:
                    return GameObjectOf(componentOrNull);
                default:
                    throw new System.Exception($"ServiceLocator.GetServiceLocator: Not supported FromWhere {where}");
            }
        }
    }
}