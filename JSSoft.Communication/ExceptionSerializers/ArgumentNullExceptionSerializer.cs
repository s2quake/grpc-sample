// MIT License
// 
// Copyright (c) 2019 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;

namespace JSSoft.Communication.ExceptionSerializers
{
    class ArgumentNullExceptionSerializer : ExceptionSerializerBase<ArgumentNullException>
    {
        private readonly static Dictionary<string, string> messageByParam = new();

        public ArgumentNullExceptionSerializer()
            : base(new Guid("429dfc10-5ee5-4fb0-93da-9e06a85ff3cc"))
        {

        }

        public override Type[] PropertyTypes => new Type[]
        {
            typeof(string),
            typeof(string)
        };

        public static readonly ArgumentNullExceptionSerializer Default = new();

        protected override ArgumentNullException CreateInstance(object[] args)
        {
            var paramName = args[0] as string;
            if (paramName != null && args[1] is string message)
                new ArgumentNullException(paramName, message);
            else if (paramName != null)
                return new ArgumentNullException(paramName);
            return new ArgumentNullException();
        }

        protected override object[] SelectProperties(ArgumentNullException e)
        {
            var paramName = e.ParamName;
            var message = e.Message == GetMessage(paramName) ? null : e.Message;
            return new object[] { paramName, message };
        }

        private static string GetMessage(string paramName)
        {
            if (paramName is not null && messageByParam.ContainsKey(paramName))
            {
                var exception = new ArgumentNullException(paramName);
                messageByParam.Add(paramName, exception.Message);
            }
            return paramName != null ? messageByParam[paramName] : null;
        }
    }
}
