using SmartLogin.QQ.Builder;
using SmartLogin.QQ.Models;
using System;

namespace SmartLogin.QQ.Provider
{
    public partial interface IQQClientBuilder
    {
        IQQClientBuilder ReceivedFriendMessage(Action<FriendMessage> action);
        IQQClientBuilder ReceivedGroupMessage(Action<GroupMessage> action);
        IQQClientBuilder ReceivedDiscussionMessage(Action<DiscussionMessage> action);
        IQQClientBuilder ReceivedMessageEchoEventArgs(Action<MessageEchoEventArgs> action);
        IQQClientBuilder ExtraLoginNeeded(Action<string> action);
        IQQClientBuilder ConnectionLost(Action<object> action);
        void Start(Action<QQClientBuilder> action = null);
    }
}
