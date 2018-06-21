﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogin.QQ.Constants
{
    public static class ApiUrlExtension
    {
        /// <summary>
        ///     发送GET请求。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url">URL。</param>
        /// <param name="args">附加的参数。</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAsync(this HttpClient client, ApiUrl url, params object[] args)
            => await client.GetAsync(url, null, args);

        /// <summary>
        /// get string from http client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(this HttpClient client, ApiUrl url, params object[] args)
            => await client.GetStringAsync(url, null, args);

        /// <summary>
        ///     发送GET请求。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url">URL。</param>
        /// <param name="allowAutoRedirect">允许自动重定向。</param>
        /// <param name="args">附加的参数。</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAsync(this HttpClient client, ApiUrl url, bool? allowAutoRedirect, params object[] args)
        {
            var referer = client.DefaultRequestHeaders.Referrer;

            if (url.Referer != null)
            {
                client.DefaultRequestHeaders.Referrer = new Uri(url.Referer);
            }

            var response = client.GetAsync(url.BuildUrl(args));
            response.Wait();
            // 复原client
            client.DefaultRequestHeaders.Referrer = referer;

            return await response;
        }


        /// <summary>
        /// get string from http client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="allowAutoRedirect"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(this HttpClient client, ApiUrl url, bool? allowAutoRedirect, params object[] args)
        {
            var referer = client.DefaultRequestHeaders.Referrer;

            if (url.Referer != null)
            {
                client.DefaultRequestHeaders.Referrer = new Uri(url.Referer);
            }

            var response = client.GetStringAsync(url.BuildUrl(args));
            response.Wait();
            // 复原client
            client.DefaultRequestHeaders.Referrer = referer;

            return await response;
        }

        /// <summary>
        /// get stream from http client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="allowAutoRedirect"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<System.IO.Stream> GetJsonAsync(this HttpClient client, ApiUrl url, bool? allowAutoRedirect, params object[] args)
        {
            var referer = client.DefaultRequestHeaders.Referrer;

            if (url.Referer != null)
            {
                client.DefaultRequestHeaders.Referrer = new Uri(url.Referer);
            }

            var response = client.GetStreamAsync(url.BuildUrl(args));
            response.Wait();
            // 复原client
            client.DefaultRequestHeaders.Referrer = referer;

            return await response;
        }

        /// <summary>
        ///     发送POST请求。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url">URL。</param>
        /// <param name="json">JSON。</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, ApiUrl url, JObject json) => client.PostAsync(url, json, -1);

        /// <summary>
        ///     发送POST请求。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url">URL。</param>
        /// <param name="json">JSON。</param>
        /// <param name="timeout">超时。</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsync(this HttpClient client, ApiUrl url, JObject json, int timeout)
        {
            var hasOrigin = client.DefaultRequestHeaders.TryGetValues("Origin", out IEnumerable<string> origin);

            if (url.Referer != null)
            {
                client.DefaultRequestHeaders.Referrer = new Uri(url.Referer);
            }
            if (client.DefaultRequestHeaders.Contains("Origin"))
            {
                client.DefaultRequestHeaders.Remove("Origin");
                client.DefaultRequestHeaders.Add("Origin", url.Origin);
            }
            else
            {
                client.DefaultRequestHeaders.Add("Origin", url.Origin);
            }

            HttpContent hc = new StringContent($"r={WebUtility.UrlEncode(json.ToString(Formatting.None))}", Encoding.UTF8);
            hc.Headers.ContentType = MediaTypeHeaderValue.Parse("application/ x-www-form-urlencoded; charset=UTF-8");
            var response = client.PostAsync(url.Url, hc);
            response.Wait();

            // 复原client
            if (hasOrigin)
            {
                client.DefaultRequestHeaders.Remove("Origin");
                client.DefaultRequestHeaders.Add("Origin", origin);
            }
            else
            {
                client.DefaultRequestHeaders.Remove("Origin");
            }

            return await response;
        }

        /// <summary>
        ///     带重试的发送。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url">URL。</param>
        /// <param name="json">JSON。</param>
        /// <param name="retryTimes">重试次数。</param>
        /// <returns></returns>
        internal static async Task<HttpResponseMessage> PostWithRetry(this HttpClient client, ApiUrl url, JObject json, int retryTimes)
        {
            Task<HttpResponseMessage> response;
            do
            {
                response = client.PostAsync(url.Url, new StringContent(json.ToString(Formatting.None),Encoding.UTF8, "application/json"));
                retryTimes++;
            } while (retryTimes >= 0 && response.Result.StatusCode != System.Net.HttpStatusCode.OK);
            return await response;
        }

        /// <summary>
        ///   带重试的发送。
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="retryTimes"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsyncWithRetry(this HttpClient client, ApiUrl url, JObject json, int retryTimes)
        {
            Task<HttpResponseMessage> response;
            do
            {
                response = client.PostAsync(url.Url, new StringContent(json.ToString(Formatting.None), Encoding.UTF8, "application/json"));
                response.Wait();
                retryTimes++;
            }
            while (retryTimes >= 0 && response.Result.StatusCode != System.Net.HttpStatusCode.OK);
            return await response;
        }


        internal static async Task<string>  RawText(this HttpResponseMessage response)
        {
            
            return  await response.Content.ReadAsStringAsync();
        }
    }
}
