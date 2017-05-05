﻿/*
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
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace MoonUnit.InternalClasses
{
    /// <summary>
    /// Output class
    /// </summary>
    internal class Output
    {
        #region Constructor

        public Output()
        {
            MethodsCalled = new List<TestMethod>();
        }

        #endregion

        #region Functions

        public void MethodCalled(MethodInfo MethodInformation, Exception Exception=null)
        {
            MethodsCalled.Add(new TestMethod(MethodInformation, Exception));
        }

        public override string ToString()
        {
            string AssemblyLocation = Assembly.GetAssembly(this.GetType()).Location;
            AssemblyName Name = AssemblyName.GetAssemblyName(AssemblyLocation);
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            Builder.Append(string.Format("<MoonUnit><Header><FileLocation>{0}</FileLocation><Version>{1}</Version></Header><Tests>", AssemblyLocation.StripIllegalXML(), Name.Version));
            foreach (TestMethod Method in MethodsCalled)
            {
                Builder.Append(Method.ToString());
            }
            Builder.Append("</Tests><Footer></Footer></MoonUnit>");
            return Builder.ToString();
        }

        #endregion

        #region Properties

        public virtual List<TestMethod> MethodsCalled { get; private set; }

        #endregion
    }
}
