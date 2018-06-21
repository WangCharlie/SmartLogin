﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartLogin.QQ.Builder;
using SmartLogin.QQ.Constants;
using SmartLogin.QQ.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SmartLogin.QQ.Models
{
    /// <summary>
    ///     好友（不含详细信息）。
    /// </summary>
    public class Friend : User, IListable, IMessageable
    {
        [JsonIgnore]
        private readonly LazyHelper<FriendCategory> _category = new LazyHelper<FriendCategory>();

        [JsonIgnore]
        private readonly LazyHelper<FriendInfo> _info = new LazyHelper<FriendInfo>();

        [JsonIgnore]
        internal QQClientBuilder Client;

        /// <inheritdoc />
        [JsonProperty("nickname")]
        public override string Nickname { get; internal set; }

        /// <summary>
        ///     备注姓名。
        /// </summary>
        [JsonProperty("markname")]
        public string Alias { get; internal set; }

        /// <summary>
        ///     QQ会员状态。
        /// </summary>
        [JsonProperty("vip")]
        public bool IsVip { get; internal set; }

        /// <summary>
        ///     会员等级。
        /// </summary>
        [JsonProperty("vipLevel")]
        public int VipLevel { get; internal set; }

        [JsonIgnore]
        internal int CategoryIndex { get; set; }

        /// <summary>
        ///     所属分组。
        /// </summary>
        [JsonIgnore]
        public FriendCategory Category
            => _category.GetValue(() => Client.Categories.Find(_ => _.Index == CategoryIndex));

        [JsonIgnore]
        private FriendInfo Info => _info.GetValue(() =>
        {
            Logger.Instance.Debug("开始获取好友信息");
            var response = Client.Client.GetAsync(ApiUrl.GetFriendInfo, Id, Client.Vfwebqq, Client.Psessionid);
            return ((JObject)Client.GetResponseJson(response.Result)["result"]).ToObject<FriendInfo>();
        });

        /// <summary>
        ///     个性签名。
        /// </summary>
        [JsonIgnore]
        public string Bio => Info.Bio;

        /// <summary>
        ///     生日。
        /// </summary>
        [JsonIgnore]
        public Birthday Birthday => Info.Birthday;

        /// <summary>
        ///     座机号码。
        /// </summary>
        [JsonIgnore]
        public string Phone => Info.Phone;

        /// <summary>
        ///     手机号码。
        /// </summary>
        [JsonIgnore]
        public string Cellphone => Info.Cellphone;

        /// <summary>
        ///     邮箱地址。
        /// </summary>
        [JsonIgnore]
        public string Email => Info.Email;

        /// <summary>
        ///     职业。
        /// </summary>
        [JsonIgnore]
        public string Job => Info.Job;

        /// <summary>
        ///     个人主页。
        /// </summary>
        [JsonIgnore]
        public string Homepage => Info.Homepage;

        /// <summary>
        ///     学校。
        /// </summary>
        [JsonIgnore]
        public string School => Info.School;

        /// <summary>
        ///     国家。
        /// </summary>
        [JsonIgnore]
        public string Country => Info.Country;

        /// <summary>
        ///     省份。
        /// </summary>
        [JsonIgnore]
        public string Province => Info.Province;

        /// <summary>
        ///     城市。
        /// </summary>
        [JsonIgnore]
        public string City => Info.City;

        /// <summary>
        ///     性别。
        /// </summary>
        [JsonIgnore]
        public string Gender => Info.Gender;

        /// <summary>
        ///     生肖。
        /// </summary>
        [JsonIgnore]
        public int Shengxiao => Info.Shengxiao;

        /// <summary>
        ///     某信息字段。意义暂不明确。
        /// </summary>
        [JsonIgnore]
        public string Personal => Info.Personal;

        /// <summary>
        ///     某信息字段。意义暂不明确。
        /// </summary>
        [JsonIgnore]
        public int VipInfo => Info.VipInfo;

        /// <inheritdoc />
        [JsonIgnore]
        public override long QQNumber => Client.GetQQNumberOf(Id);

        /// <inheritdoc />
        [JsonProperty("userId")]
        public override long Id { get; internal set; }

        /// <inheritdoc />
        /// <param name="content">消息内容。</param>
        public void Message(string content)
        {
            Client.Message(TargetType.Friend, Id, content);
        }

        string IMessageable.Name => Nickname;

        /// <inheritdoc />
        protected bool Equals(Friend other)
        {
            return base.Equals(other) && Id == other.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Friend)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Id.GetHashCode();

        internal static List<Friend> GetList(QQClientBuilder client)
        {
            Logger.Instance.Debug("开始获取好友列表");
            var response = client.Client.PostAsync(ApiUrl.GetFriendList,
                new JObject { { "vfwebqq", client.Vfwebqq }, { "hash", client.Hash } });
            var result = (JObject)client.GetResponseJson(response.Result)["result"];
            //获得好友信息
            var friendDictionary = QQClientBuilder.ParseFriendDictionary(result);
            var friends = result["friends"] as JArray;
            for (var i = 0; friends != null && i < friends.Count; i++)
            {
                var item = (JObject)friends[i];
                friendDictionary[item["uin"].Value<long>()].CategoryIndex = item["categories"].Value<int>();
            }
            var value = friendDictionary.Select(_ => _.Value).ToList();
            value.ForEach(_ => _.Client = client);
            return value;
        }

        /// <inheritdoc />
        public static bool operator ==(Friend left, Friend right) => left?.Id == right?.Id;

        /// <inheritdoc />
        public static bool operator !=(Friend left, Friend right) => !(left == right);
    }
}
