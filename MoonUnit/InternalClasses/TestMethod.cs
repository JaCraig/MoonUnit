/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonUnit.Exceptions;
using System.Reflection;
using MoonUnit.BaseClasses;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace MoonUnit.InternalClasses
{
    /// <summary>
    /// Holds information about test methods
    /// </summary>
    internal class TestMethod
    {
        #region Constructor


        public TestMethod(MethodInfo MethodInformation,Exception Exception)
        {
            this.MethodInformation = MethodInformation;
            this.Exception = Exception;
        }

        #endregion

        #region Functions

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append(string.Format("<Test><Class name=\"{0}\" /><Method name=\"{1}\" />",
                                            MethodInformation.DeclaringType.Name.StripIllegalXML(),
                                            MethodInformation.Name.StripIllegalXML()));
            if (Exception == null)
                Builder.Append("<Passed />");
            else if (Exception is BaseException)
                Builder.Append(string.Format("<Failed>{0}</Failed>", Exception.ToString()));
            else
                Builder.Append(string.Format("<Failed><Expected>{0}</Expected><Result>{1}</Result><ErrorText>{2}</ErrorText><Trace>{3}</Trace><ErrorType>{4}</ErrorType></Failed>",
                                        "",
                                        "",
                                        Exception.Message.StripIllegalXML(),
                                        Exception.StackTrace.StripIllegalXML(),
                                        Exception.GetType().Name.StripIllegalXML()));
            Builder.Append("</Test>");
            return Builder.ToString();
        }

        #endregion

        #region Properties

        public virtual MethodInfo MethodInformation { get; private set; }

        public virtual Exception Exception { get; set; }

        #endregion
    }
}
