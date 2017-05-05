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
#endregion

namespace MoonUnit.Attributes
{
    /// <summary>
    /// Attribute used to denote a test function
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestAttribute : Attribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TimeOut">Time out period (in ms)</param>
        public TestAttribute(long TimeOut=0)
            : base()
        {
            Skip = false;
            this.TimeOut = TimeOut;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ReasonForSkipping">Reason for skipping this test</param>
        /// <param name="TimeOut">Time out period (in ms)</param>
        public TestAttribute(string ReasonForSkipping,long TimeOut=0)
            : base()
        {
            this.Skip = true;
            this.ReasonForSkipping = ReasonForSkipping;
            this.TimeOut = TimeOut;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Determines if the test should be skipped
        /// </summary>
        public bool Skip { get; private set; }

        /// <summary>
        /// The reason for skipping this test
        /// </summary>
        public string ReasonForSkipping { get; private set; }

        /// <summary>
        /// If it takes longer than this, consider it a timeout/failed test
        /// </summary>
        public long TimeOut { get; private set; }

        #endregion
    }
}
