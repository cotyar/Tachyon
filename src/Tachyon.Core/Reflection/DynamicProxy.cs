#region copyright
// Copyright 2011 Olivier Deheurles
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Tachyon.Core.Reflection
{
    static class DynamicProxy
    {
        private static readonly ModuleBuilder ModuleBuilder = AssemblyBuilder
            .DefineDynamicAssembly(new AssemblyName("Tachyon.Core.DynamicAssembly"), AssemblyBuilderAccess.Run)
            .DefineDynamicModule("Tachyon.Core.DynamicAssemblyModule");

        private static readonly TypeDictionary<Type> Proxies = new TypeDictionary<Type>(10);

        public static TInterface StructWrap<TInterface>(TInterface target)
        {
            var tTarget = target.GetType();

            if (tTarget.IsValueType) return target;

            Type tProxy;
            lock (Proxies)
            {
                if (!Proxies.TryGetValue(tTarget, out tProxy))
                {
                    tProxy = GenerateStructProxyType(tTarget);
                    Proxies.Add(tTarget, tProxy);
                }
            }

            return (TInterface) Activator.CreateInstance(tProxy, target);
        }

        private static Type GenerateStructProxyType(Type targetType)
        {
            if (!targetType.IsVisible)
                return null;

            var typeBuilder = ModuleBuilder.DefineType($"StructProxy_{targetType.Name}_{Guid.NewGuid():N}", TypeAttributes.Public, typeof(ValueType));

            var field = typeBuilder.DefineField("_target", targetType, FieldAttributes.Private);

            GenerateConstructor(targetType, typeBuilder, field);
            
            foreach (var interfaceType in targetType.GetInterfaces())
            {
                if (interfaceType.IsVisible)
                    GenerateInterfaceImplementation(interfaceType, targetType, typeBuilder, field);
            }

            return typeBuilder.CreateTypeInfo();
        }


        private static void GenerateConstructor(Type targetType, TypeBuilder typeBuilder, FieldBuilder field)
        {
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { targetType });

            var constructorGenerator = constructor.GetILGenerator();
            constructorGenerator.Emit(OpCodes.Ldarg_0);
            constructorGenerator.Emit(OpCodes.Ldarg_1);
            constructorGenerator.Emit(OpCodes.Stfld, field);
            constructorGenerator.Emit(OpCodes.Ret);
        }

        private static void GenerateInterfaceImplementation(Type interfaceType, Type targetType, TypeBuilder typeBuilder, FieldBuilder field)
        {
            typeBuilder.AddInterfaceImplementation(interfaceType);

            var interfaceMap = targetType.GetInterfaceMap(interfaceType);

            for (var index = 0; index < interfaceMap.InterfaceMethods.Length; index++)
            {
                var interfaceMethodInfo = interfaceMap.InterfaceMethods[index];
                var targetMethodInfo = interfaceMap.TargetMethods[index];
                var parameters = interfaceMethodInfo.GetParameters();
                var parameterTypes = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameterTypes[i] = parameters[i].ParameterType;
                }

                var method = typeBuilder.DefineMethod(interfaceMethodInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final, interfaceMethodInfo.ReturnType, parameterTypes);

                if (targetMethodInfo.IsGenericMethod)
                {
                    var genericArguments = targetMethodInfo.GetGenericArguments();
                    var genericParameters = new string[genericArguments.Length];
                    for (int i = 0; i < genericArguments.Length; i++)
                    {
                        genericParameters[i] = "T" + i;
                    }
                    method.DefineGenericParameters(genericParameters);
                }

                method.SetImplementationFlags(method.GetMethodImplementationFlags() | MethodImplAttributes.AggressiveInlining);

                var methodGenerator = method.GetILGenerator();
                methodGenerator.Emit(OpCodes.Ldarg_0);
                methodGenerator.Emit(OpCodes.Ldfld, field);

                for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                {
                    methodGenerator.Emit(OpCodes.Ldarg_S, (byte)parameterIndex + 1);
                }

                methodGenerator.Emit(OpCodes.Call, targetMethodInfo);
                methodGenerator.Emit(OpCodes.Ret);
            }
        }
    }
}