﻿using Microsoft.AspNetCore.SignalR;

namespace Mango.Web.Utility
{
    public class SD
    {
        public static string CouponAPIBase { get; set; }
        public enum ApiType
        {
            Get,
            Post,
            PUT,
            Delete
        }
    }    
}
