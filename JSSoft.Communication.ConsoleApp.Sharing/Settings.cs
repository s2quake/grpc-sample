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


/* Server' ������Ʈ���� ���յ��� ���� ���� ����
����:
using System;
using System.ComponentModel;
using JSSoft.Communication;
using JSSoft.Library.Commands;
����:
using JSSoft.Communication;
using JSSoft.Library.Commands;
using System;
using System.ComponentModel;
*/

/* Server' ������Ʈ���� ���յ��� ���� ���� ����
����:
using System;
using System.ComponentModel;
using JSSoft.Communication;
using JSSoft.Library.Commands;
����:
using JSSoft.Communication;
using JSSoft.Library.Commands;
using System;
using System.ComponentModel;
*/

/* Client' ������Ʈ���� ���յ��� ���� ���� ����
����:
using System;
using System.ComponentModel;
using JSSoft.Communication;
using JSSoft.Library.Commands;
����:
using JSSoft.Communication;
using JSSoft.Library.Commands;
using System;
using System.ComponentModel;
*/
using JSSoft.Library.Commands;
using System;
using System.ComponentModel;

namespace JSSoft.Communication.ConsoleApp
{
    public class Settings
    {
        [CommandProperty]
        [DefaultValue(ServiceContextBase.DefaultPort)]
        public int Port
        {
            get; set;
        }

        [CommandProperty]
        [DefaultValue(ServiceContextBase.DefaultHost)]
        public string Host
        {
            get; set;
        }

        [CommandProperty]
        public bool Verbose
        {
            get; set;
        }

        public static Settings CreateFromCommandLine()
        {
            var settings = new Settings();
            var parser = new CommandLineParser(settings);
            parser.Parse(Environment.CommandLine);
            return settings;
        }
    }
}