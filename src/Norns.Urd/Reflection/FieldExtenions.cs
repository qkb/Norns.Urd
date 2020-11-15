﻿using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Norns.Urd.Reflection
{
    public static class FieldExtenions
    {
        public static Action<object, object> CreateSetter(this FieldInfo field)
        {
            var method = new DynamicMethod(Guid.NewGuid().ToString("N"), typeof(void), new Type[] { typeof(object), typeof(object) });
            var il = method.GetILGenerator();
            il.EmitLoadArg(0);
            il.EmitLoadArg(1);
            il.EmitConvertObjectTo(field.FieldType);
            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);
            return (Action<object, object>)method.CreateDelegate(typeof(Action<object, object>));
        }
        public static Func<object, object> CreateGetter(this FieldInfo field)
        {
            var method = new DynamicMethod(Guid.NewGuid().ToString("N"), typeof(object), new Type[] { typeof(object) });
            var il = method.GetILGenerator();
            il.EmitLoadArg(0);
            il.Emit(OpCodes.Ldfld, field);
            il.EmitConvertToObject(field.FieldType);
            il.Emit(OpCodes.Ret);
            return (Func<object, object>)method.CreateDelegate(typeof(Func<object, object>));
        }
    }
}