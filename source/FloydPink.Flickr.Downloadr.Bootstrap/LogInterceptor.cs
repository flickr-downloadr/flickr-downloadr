using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Castle.DynamicProxy;
using FloydPink.Flickr.Downloadr.Repository.Extensions;
using log4net;

namespace FloydPink.Flickr.Downloadr.Bootstrap
{
    public class LogInterceptor : IInterceptor
    {
        private readonly ILog _log;

        public LogInterceptor(ILog log)
        {
            _log = log;
        }

        public ILog Log
        {
            get { return _log; }
        }

        public void Intercept(IInvocation invocation)
        {
            if (Log.IsDebugEnabled) Log.Debug(CreateInvocationLogString("Called", invocation));
            try
            {
                invocation.Proceed();

                if (Log.IsDebugEnabled)
                {
                    var returnType = invocation.Method.ReturnType;
                    if (returnType != typeof(void))
                    {
                        var returnValue = invocation.ReturnValue;
                        if (returnType == typeof(Task))
                        {
                            Log.Debug("Returning with a task.");
                        }
                        else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                        {
                            Log.Debug("Returning with a generic task.");
                            var task = (Task)returnValue;
                            task.ContinueWith((antecedent) =>
                                                  {
                                                      var taskDescriptor = CreateInvocationLogString("Task from", invocation);
                                                      var result =
                                                          antecedent.GetType()
                                                                    .GetProperty("Result")
                                                                    .GetValue(antecedent, null);
                                                      Log.Debug(taskDescriptor + " returning with: " + result);
                                                  });
                        }
                        else
                        {
                            Log.Debug("Returning with: " + returnValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error(CreateInvocationLogString("ERROR", invocation), ex);
                throw;
            }
        }

        public static string CreateInvocationLogString(string operation, IInvocation invocation)
        {
            var sb = new StringBuilder(100);
            sb.AppendFormat("{0}: {1}.{2}(", operation, invocation.TargetType.Name, invocation.Method.Name);
            foreach (object argument in invocation.Arguments)
            {
                String argumentDescription = argument == null ? "null" : DumpObject(argument);
                sb.Append(argumentDescription).Append(",");
            }
            if (invocation.Arguments.Any()) sb.Length--;
            sb.Append(")");
            return sb.ToString();
        }

        private static string DumpObject(object argument)
        {
            Type objtype = argument.GetType();
            if (objtype == typeof(String) || objtype.IsPrimitive || !objtype.IsClass)
                return argument.ToString();

            return ToJson(argument);
        }

        public static string ToJson(object value)
        {
            try
            {
                var serializedObject = (new JavaScriptSerializer()).Serialize(value);
                return serializedObject;
            }
            catch (Exception)
            {
                return value.ToString();
            }
        }
    }
}