using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Contacts
{
    public static class ApiRoutes
    {
        public const string Root = "Api";
        public const string Version = "V1";
        public const string Base = Root + "/" + Version;
        public static class posts
        {
            public const string GetAll = Base + "/Posts";
            public const string Get = Base + "/Posts/{postId}";
            public const string Create = Base + "/Posts";
            public const string Update = Base + "/Posts/{postId}";
            public const string Delete = Base + "/Posts/{postId}";

        }
        public static class Identity
        {
            public const string Login = Base + "/Identity/Login";
            public const string Register = Base + "/Identity/Register";
        }
    }
}
