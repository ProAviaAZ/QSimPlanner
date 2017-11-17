﻿namespace QSP.UI.Views
{
    public interface ICanShowUserMessage
    {
        void ShowMessage(string s, MessageLevel lvl);
    }
    
    public enum MessageLevel
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }
}