﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartLogin.QQ.Builder;
using SmartLogin.QQ.Utils;

namespace SmartLogin.QQ.Models
{
    /// <summary>
    ///     讨论组消息。
    /// </summary>
    public class DiscussionMessage : IMessage
    {
        [JsonIgnore]
        private readonly LazyHelper<Discussion> _discussion = new LazyHelper<Discussion>();

        [JsonIgnore]
        private readonly LazyHelper<DiscussionMember> _sender = new LazyHelper<DiscussionMember>();

        [JsonIgnore]
        internal QQClientBuilder Client;

        /// <summary>
        ///     讨论组ID。
        /// </summary>
        [JsonProperty("did")]
        internal long DiscussionId { get; set; }

        /// <summary>
        ///     来源讨论组。
        /// </summary>
        [JsonIgnore]
        public Discussion Discussion => _discussion.GetValue(() => Client.Discussions.Find(_ => _.Id == DiscussionId));

        /// <summary>
        ///     字体。
        /// </summary>
        [JsonProperty("content_font")]
        internal Font Font { get; set; }

        /// <summary>
        ///     用于parse消息和字体的对象。
        /// </summary>
        [JsonProperty("content")]
        internal JArray ContentAndFont
        {
            set
            {
                Font = ((JArray)value.First).Last.ToObject<Font>();
                value.RemoveAt(0);
                foreach (var shit in value)
                    Content += StringHelper.ParseEmoticons(shit);
            }
        }

        /// <summary>
        ///     发送者ID。
        /// </summary>
        [JsonProperty("send_uin")]
        internal long SenderId { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public DiscussionMember Sender => _sender.GetValue(() => Discussion.Members.Find(_ => _.Id == SenderId));

        [JsonIgnore]
        User IMessage.Sender => Sender;

        /// <inheritdoc />
        [JsonProperty("time")]
        public long Timestamp { get; set; }

        /// <inheritdoc />
        [JsonProperty("content_text")]
        public string Content { get; set; }

        /// <inheritdoc />
        /// <param name="content">回复内容。</param>
        public void Reply(string content)
        {
            Client.Message(TargetType.Discussion, DiscussionId, content);
        }

        /// <inheritdoc />
        IMessageable IMessage.RepliableTarget => Discussion;
    }
}
