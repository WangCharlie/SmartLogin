using SmartLogin.QQ.Builder;
using SmartLogin.QQ.Provider;
using System;

namespace SmartLogin.QQ
{
    public static partial class QQClient
    {
        public static IQQClientBuilder Login(Action<byte[]> action)
        {
            return new QQClientBuilder(action);
        }

        public static IQQClientBuilder Login(Action<byte[]> action, Func<string, LoginResult> json)
        {
            return new QQClientBuilder(action, json);
        }

        public static IQQClientBuilder StartListen(int port)
        {
            return new QQClientBuilder(port);
        }
    }
}