using SmartLogin.QQ.Constants;
using SmartLogin.QQ.Models;
using System;
using System.Net.Http;

namespace SmartLogin.QQ.Builder
{
    public partial class QQClientBuilder
    {

        #region Public function

        public QQClientBuilder(Action<byte[]> bytes)
        {
            this._bytes = bytes;
        }

        public QQClientBuilder(Action<byte[]> bytes, Func<string, LoginResult> json)
        {
            this._bytes = bytes;
            this.json = json;
        }

        public QQClientBuilder(int port)
        {
            this._port = port;
        }

        public void Start(Action<QQClientBuilder> action)
        {
            
            Start();
            action(this);
        }

        public void Start()
        {
            Logger.Instance.OutputType = OutputType.Console;
            Handler.UseCookies = true;
            Handler.CookieContainer = Cookies;
            Handler.AllowAutoRedirect = true;
            Client = new HttpClient(Handler);
            Client.DefaultRequestHeaders.Add("User-Agent", ApiUrl.UserAgent);
            Client.DefaultRequestHeaders.Add("KeepAlive", "true");
            _cache = new CacheDepot(CacheTimeout);
            _myInfoCache = new Cache<FriendInfo>(CacheTimeout);
            _qqNumberCache = new CacheDictionary<long, long>(CacheTimeout);
            Start(this._bytes);
            //Start(this.json);
        }

        #endregion

    }
}
