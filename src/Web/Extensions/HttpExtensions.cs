// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.AspNetCore.Http;
using System;

namespace Microsoft.AspNetCore.Mvc
{
    public static class HttpExtensions
    {
        public static string BuildCallbackLink(this HttpRequest request, string path, int orderId)
        {
            var uri = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = path,
                Query = $"order={orderId}"
            };

            if(request.Host.Port != null)
            {
                uri.Port = (int)request.Host.Port;
            }

            return uri.ToString();
        }
    }
}
