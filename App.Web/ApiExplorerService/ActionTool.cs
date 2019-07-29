using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.ApiExplorerService
{
    public class ActionTool
    {
        public static string GetUid(ActionDescriptor desc)
        {
            var controllerActionDescriptor = desc as ControllerActionDescriptor;
            var controllerName = controllerActionDescriptor.ControllerTypeInfo.FullName;
            var methodInfoList = controllerActionDescriptor.MethodInfo.ToString().Split(" ").ToList();
            methodInfoList[0] = controllerName + ".";
            var actionName = string.Join(' ', methodInfoList).Replace(" ", string.Empty);
            return $"{AppWebContext.Instance.SystemIndex}:{actionName.GetHashCode()}";
        }
    }
}
