using SmartLogin.QQ.Models;
using SmartLogin.QQ.Provider;
using System;

namespace SmartLogin.QQ.Builder
{
    public partial class QQClientBuilder: IQQClientBuilder
    {
        #region FriendMessage

        private Action<FriendMessage> actionFriendMessage;
        public IQQClientBuilder ReceivedFriendMessage(Action<FriendMessage> action)
        {
            this.actionFriendMessage = (sender) =>
            {
                action(sender);
            };
            return this;
           
        }

        #endregion

        #region GroupMessage

        private Action<GroupMessage> actionGroupMessage;
        public IQQClientBuilder ReceivedGroupMessage(Action<GroupMessage> action)
        {
            this.actionGroupMessage = (sender) =>
            {
                action(sender);
            };
            return this;

        }

        #endregion

        #region DiscussionMessage

        private Action<DiscussionMessage> actionDiscussionMessage;
        public IQQClientBuilder ReceivedDiscussionMessage(Action<DiscussionMessage> action)
        {
            this.actionDiscussionMessage = (sender) =>
            {
                action(sender);
            };
            return this;

        }

        #endregion

        #region MessageEchoEventArgs

        private Action<MessageEchoEventArgs> actionMessageEchoEventArgs;
        public IQQClientBuilder ReceivedMessageEchoEventArgs(Action<MessageEchoEventArgs> action)
        {
            this.actionMessageEchoEventArgs = (sender) =>
            {
                action(sender);
            };
            return this;

        }

        #endregion

        #region MessageEchoEventArgs

        private Action<string> actionExtraLoginNeeded;
        public IQQClientBuilder ExtraLoginNeeded(Action<string> action)
        {
            this.actionExtraLoginNeeded = (sender) =>
            {
                action(sender);
            };
            return this;

        }

        #endregion  

        #region ConnectionLost

        private Action<Object> actionConnectionLost;
        public IQQClientBuilder ConnectionLost(Action<object> action)
        {
            this.actionConnectionLost = (sender) =>
            {
                action(sender);
            };
            return this;

        }

        #endregion  
       
    }
}
