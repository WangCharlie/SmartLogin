﻿namespace SmartLogin.QQ.Models
{
    /// <summary>
    ///     表示消息的接口。
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        ///     消息时间戳。
        /// </summary>
        long Timestamp { get; }

        /// <summary>
        ///     消息文字内容。
        /// </summary>
        string Content { get; }

        /// <summary>
        ///     消息发送者。
        /// </summary>
        User Sender { get; }

        /// <summary>
        ///     可回复的对象。
        /// </summary>
        IMessageable RepliableTarget { get; }

        /// <summary>
        ///     回复该消息。
        /// </summary>
        /// <param name="content">回复内容。</param>
        void Reply(string content);
    }
}
