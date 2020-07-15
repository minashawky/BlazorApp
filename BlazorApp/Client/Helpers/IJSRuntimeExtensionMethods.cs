using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Client.Helpers
{
    public static class IJSRuntimeExtensionMethods
    {
        public static async ValueTask<bool> Confirm(this IJSRuntime js, string message)
        {
            return await js.InvokeAsync<bool>("confirm", message);
        }

        public static async ValueTask StartBasicAgoraCall(this IJSRuntime js)
        {
            await js.InvokeVoidAsync("startBasicCall");
        }

        public static async ValueTask LeaveCall(this IJSRuntime js)
        {
            await js.InvokeVoidAsync("leaveCall");
        }

        public static async ValueTask MyFunction(this IJSRuntime js, string message)
        {
            await js.InvokeVoidAsync("my_function", message);
        }
    }
}
