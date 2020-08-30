using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Dlog.Api.Middlewares.ServerResponse
{
    public class ResponseModelCreator
    {
        private Dictionary<Type, PropertyInfo> props;
        private Type resModelType;
        public ResponseModelCreator()
        {
            props = new Dictionary<Type, PropertyInfo>();
            var tb = CreateTypeBuilder();
            var propNames = FetchProperties();
            foreach (var item in propNames)
            {
                CreateProperty(tb, item.Value, item.Key);
            }
            resModelType = tb.CreateType();
            foreach(var item in propNames)
            {
                props.Add(item.Key, resModelType.GetProperty(item.Value));
            }
        }

        private TypeBuilder CreateTypeBuilder()
        {
            var assName = Assembly.GetExecutingAssembly().GetName();
            AssemblyBuilder dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(assName, AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder mb = dynamicAssembly.DefineDynamicModule(assName.Name);
            TypeBuilder tb = mb.DefineType("ResponseModel", TypeAttributes.Public);
            return tb;
        }

        private Dictionary<Type, string> FetchProperties()
        {
            var propNames = new Dictionary<Type, string>();
            var ass = Assembly.GetExecutingAssembly();
            foreach (var item in ass.GetTypes())
            {
                var attrs = item
                    .GetCustomAttributes(typeof(ResponsePropertyNameAttribute))
                    .Select(p => p as ResponsePropertyNameAttribute)
                    .ToArray();
                if (attrs.Length == 0)
                {
                    continue;
                }
                foreach (var attr in attrs)
                {
                    var key = attr.Type ?? item;
                    propNames.Add(key, attr.Name);
                }
            }
            return propNames;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);


        }
    }

}
