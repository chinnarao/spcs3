namespace Share.Constants
{
    public static class Constants
    {
        public const string SP_DAL_QUALIFIEDNAME = "SP.DAL.Models.{0}, SP.DAL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        public static readonly string Entry = "_Entry";
        public static readonly string AD_HTML_FILE_TEMPLATE = "_AdHtmlFileTemplate"; //this is for chache key in html file in file project used
        public static readonly string Article_HTML_FILE_TEMPLATE = "_ArticleHtmlFileTemplate"; //this is for chache key in html file in file project used
    }

    public static class LoggingEvents
    {
        public static readonly int GENERATE_ITEMS = 1000;
        public static readonly int LIST_ITEMS = 1001;
        public static readonly int GET_ITEM = 1002;
        public static readonly int INSERT_ITEM = 1003;
        public static readonly int UPDATE_ITEM = 1004;
        public static readonly int DELETE_ITEM = 1005;
        public static readonly int GET_ITEM_NOTFOUND = 4000;
        public static readonly int UPDATE_ITEM_NOTFOUND = 4001;
    }
}
