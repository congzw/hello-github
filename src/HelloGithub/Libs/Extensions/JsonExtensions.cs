using System;
using Newtonsoft.Json;

namespace HelloGithub.Libs.Extensions
{
    public static class JsonExtensions
    {
        /// <summary>
        /// object as json
        /// </summary>
        /// <param name="model"></param>
        /// <param name="indented"></param>
        /// <returns></returns>
        public static string ToJson(this object model, bool indented)
        {
            return JsonConvert.SerializeObject(model, indented ? Formatting.Indented : Formatting.None);
        }

        /// <summary>
        /// json as T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="failThrowEx"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string json, bool failThrowEx = false)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                if (failThrowEx)
                {
                    throw;
                }
                //ignored
                return default(T);
            }
        }

        /// <summary>
        /// json as object
        /// </summary>
        /// <param name="json"></param>
        /// <param name="defaultValue"></param>
        /// <param name="failThrowEx"></param>
        /// <returns></returns>
        public static object FromJson(this string json, object defaultValue = null, bool failThrowEx = false)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return defaultValue;
            }
            try
            {
                return JsonConvert.DeserializeObject(json);
            }
            catch (Exception)
            {
                if (failThrowEx)
                {
                    throw;
                }
                //ignored
                return defaultValue;
            }
        }
    }
}
