﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Swashbuckle.SwaggerGen.Generator
{
    public static class ApiDescriptionExtensions
    {
        public static string FriendlyId(this ApiDescription apiDescription)
        {
            var parts = (apiDescription.RelativePathSansQueryString() + "/" + apiDescription.HttpMethod.ToLower())
                .Split('/');

            var builder = new StringBuilder();
            foreach (var part in parts) 
            {
                var trimmed = part.Trim('{', '}');

                builder.AppendFormat("{0}{1}",
                    (part.StartsWith("{") ? "By" : string.Empty),
                    trimmed.ToTitleCase()
                );
            }

            return builder.ToString();
        }

        public static IEnumerable<string> Produces(this ApiDescription apiDescription)
        {
            return apiDescription.SupportedResponseTypes
                .SelectMany(r => r.ApiResponseFormats)
                .Select(format => format.MediaType)
                .Distinct();
        }

        public static string RelativePathSansQueryString(this ApiDescription apiDescription)
        {
            return apiDescription.RelativePath.Split('?').First();
        }

        public static bool IsObsolete(this ApiDescription apiDescription)
        {
            return apiDescription.GetActionAttributes().OfType<ObsoleteAttribute>().Any();
        }

        public static IEnumerable<object> GetControllerAttributes(this ApiDescription apiDescription)
        {
            var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
            return (actionDescriptor != null)
                ? actionDescriptor.ControllerTypeInfo.GetCustomAttributes(false)
                : Enumerable.Empty<object>();
        }

        public static IEnumerable<object> GetActionAttributes(this ApiDescription apiDescription)
        {
            var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
            return (actionDescriptor != null)
                ? actionDescriptor.MethodInfo.GetCustomAttributes(false)
                : Enumerable.Empty<object>();
        }
    }
}