﻿using Norns.Urd.Proxy;
using System;
using Xunit;

namespace Norns.Urd.UT
{
    public class FieldsTestClass
    {
        private readonly int PrivateInt;
        protected int ProtectedInt;
        protected internal int ProtectedInternalInt;
        protected internal int InternalProtectedInt;
        public int PublicInt;
        public static int PublicStaticInt;
    }

    public class FieldsTest
    {
        private readonly IProxyCreator creator;

        public FieldsTest()
        {
            var (c, _, _) = ProxyCreatorUTHelper.InitPorxyCreator();
            creator = c;
        }

        [Fact]
        public void InheritWhenNoConstructorsInterface()
        {
            var proxyType = creator.CreateProxyType(typeof(FieldsTestClass));
            Assert.Equal("FieldsTestClass_Proxy_Inherit", proxyType.Name);
            var v = Activator.CreateInstance(proxyType) as FieldsTestClass;
            Assert.NotNull(v);
            Assert.Equal(0, v.PublicInt);
            Assert.Equal(0, v.ProtectedInternalInt);
        }
    }
}