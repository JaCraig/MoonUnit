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
using MoonUnit.BaseClasses;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace MoonUnit.Exceptions
{
    /// <summary>
    /// Exception thrown if a value is not within a specified range
    /// </summary>
    public class NotBetween : BaseException
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ExceptionText">Exception Text</param>
        /// <param name="Result">Actual result</param>
        /// <param name="Low">Low value (inclusive)</param>
        /// <param name="High">High value (inclusive)</param>
        public NotBetween(object Result,object Low,object High, string ExceptionText)
            : base(null, Result, ExceptionText)
        {
            this.Low = Low;
            this.High = High;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Low value
        /// </summary>
        public virtual object Low { get; set; }

        /// <summary>
        /// High value
        /// </summary>
        public virtual object High { get; set; }

        #endregion

        #region Functions

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append(string.Format("<Expected>{0}</Expected><Result>{1}</Result><ErrorText>{2}</ErrorText><Trace>{3}</Trace><ErrorType>{4}</ErrorType>",
                                        ((Low == null) ? "" : Low.ToString().StripIllegalXML()) + " - " +
                                        ((High == null) ? "" : High.ToString().StripIllegalXML()),
                                        (Result == null) ? "" : Result.ToString().StripIllegalXML(),
                                        this.Message.StripIllegalXML(),
                                        this.StackTrace.StripIllegalXML(),
                                        this.GetType().Name.StripIllegalXML()));
            return Builder.ToString();
        }

        #endregion
    }
}