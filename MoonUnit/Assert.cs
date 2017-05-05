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
using System.Collections;
using System.Text.RegularExpressions;
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace MoonUnit
{
    /// <summary>
    /// Used to make assertions
    /// </summary>
    public static class Assert
    {
        #region Functions

        #region Between

        /// <summary>
        /// Determines if a value is between two other values
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Result">Result</param>
        /// <param name="Low">Low value (inclusive)</param>
        /// <param name="High">High value (inclusive)</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Between<T>(T Result, T Low, T High, 
            string UserFailedMessage = "Assert.Between() Failed") where T : IComparable
        {
            Between(Result, Low, High, new InternalClasses.Comparer<T>());
        }

        /// <summary>
        /// Determines if a value is between two other values
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Result">Result</param>
        /// <param name="Low">Low value (inclusive)</param>
        /// <param name="High">High value (inclusive)</param>
        /// <param name="Comparer">Compares the values</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Between<T>(T Result, T Low, T High, IComparer<T> Comparer, 
            string UserFailedMessage = "Assert.Between() Failed") where T : IComparable
        {
            if (Comparer.Compare(High, Result) < 0 || Comparer.Compare(Result, Low) < 0)
                throw new NotBetween(Result, Low, High, UserFailedMessage);
        }

        #endregion

        #region Contains

        /// <summary>
        /// Determines if a collection contains an object
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Expected">Expected object</param>
        /// <param name="Collection">Collection object</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Contains<T>(T Expected, IEnumerable<T> Collection, string UserFailedMessage = "Assert.Contains() Failed")
        {
            Contains(Expected, Collection, new InternalClasses.EqualityComparer<T>(), UserFailedMessage);
        }

        /// <summary>
        /// Determines if a collection contains an object
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Expected">Expected object</param>
        /// <param name="Collection">Collection object</param>
        /// <param name="Comparer">Comparer used to compare the objects</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Contains<T>(T Expected, IEnumerable<T> Collection, IEqualityComparer<T> Comparer, string UserFailedMessage = "Assert.Contains() Failed")
        {
            if (Collection == null)
                throw new DoesNotContain(Expected, UserFailedMessage);
            foreach (T Object in Collection)
            {
                if (Comparer.Equals(Expected, Object))
                    return;
            }
            throw new DoesNotContain(Expected, UserFailedMessage);
        }

        /// <summary>
        /// Determines if a collection contains an object
        /// </summary>
        /// <param name="Expected">Expected value</param>
        /// <param name="Actual">Actual value</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Contains(string Expected, string Actual, string UserFailedMessage = "Assert.Contains() Failed")
        {
            Contains(Expected, Actual, StringComparison.CurrentCulture, UserFailedMessage);
        }

        /// <summary>
        /// Determines if a collection contains an object
        /// </summary>
        /// <param name="Expected">Expected value</param>
        /// <param name="Actual">Actual value</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Contains(string Expected, string Actual, StringComparison ComparisonType, string UserFailedMessage = "Assert.Contains() Failed")
        {
            if (string.IsNullOrEmpty(Expected) && string.IsNullOrEmpty(Actual))
                return;
            if (Actual == null || Actual.IndexOf(Expected, ComparisonType) < 0)
                throw new DoesNotContain(Expected, Actual, UserFailedMessage);
        }

        #endregion

        #region Do

        /// <summary>
        /// Calls the specified delegate and throws any errors that it receives
        /// </summary>
        /// <param name="Delegate">Delegate to call</param>
        public static void Do(VoidDelegate Delegate)
        {
            Delegate();
        }

        /// <summary>
        /// Calls the specified delegate and throws any errors that it receives 
        /// and returns a value
        /// </summary>
        /// <param name="Delegate">Delegate to call</param>
        public static T Do<T>(ReturnObjectDelegate<T> Delegate)
        {
            return Delegate();
        }

        #endregion

        #region DoesNotContain

        /// <summary>
        /// Determines if a collection does not contain an object
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Expected">Expected object</param>
        /// <param name="Collection">Collection object</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void DoesNotContain<T>(T Expected, IEnumerable<T> Collection, string UserFailedMessage = "Assert.DoesNotContain() Failed")
        {
            DoesNotContain(Expected, Collection, new InternalClasses.EqualityComparer<T>(), UserFailedMessage);
        }

        /// <summary>
        /// Determines if a collection does not contain an object
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Expected">Expected object</param>
        /// <param name="Collection">Collection object</param>
        /// <param name="Comparer">Comparer used to compare the objects</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void DoesNotContain<T>(T Expected, IEnumerable<T> Collection, IEqualityComparer<T> Comparer, string UserFailedMessage = "Assert.DoesNotContain() Failed")
        {
            if (Collection == null)
                return;
            foreach (T Object in Collection)
            {
                if (Comparer.Equals(Expected, Object))
                    throw new DoesContain(Expected, UserFailedMessage);
            }
        }

        /// <summary>
        /// Determines if a collection does not contain an object
        /// </summary>
        /// <param name="Expected">Expected value</param>
        /// <param name="Actual">Actual value</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void DoesNotContain(string Expected, string Actual, string UserFailedMessage = "Assert.DoesNotContain() Failed")
        {
            DoesNotContain(Expected, Actual, StringComparison.CurrentCulture, UserFailedMessage);
        }

        /// <summary>
        /// Determines if a collection contains an object
        /// </summary>
        /// <param name="Expected">Expected value</param>
        /// <param name="Actual">Actual value</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void DoesNotContain(string Expected, string Actual, StringComparison ComparisonType, string UserFailedMessage = "Assert.DoesNotContain() Failed")
        {
            if (string.IsNullOrEmpty(Expected) && string.IsNullOrEmpty(Actual))
                return;
            if (Actual == null || Actual.IndexOf(Expected, ComparisonType) >= 0)
                throw new DoesContain(Expected, Actual, UserFailedMessage);
        }

        #endregion

        #region DoesNotThrow

        /// <summary>
        /// Used when a specific type of exception is not expected
        /// </summary>
        /// <typeparam name="T">Type of the exception</typeparam>
        /// <param name="Delegate">Delegate called to test</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void DoesNotThrow<T>(VoidDelegate Delegate, string UserFailedMessage = "Assert.DoesNotThrow<T>() Failed")
        {
            try
            {
                Delegate();
            }
            catch (Exception e)
            {
                if (e is T)
                    throw new DoesNotThrow(typeof(T), e, UserFailedMessage);
            }
        }

        /// <summary>
        /// Used when a specific type of exception is not expected and a return value is expected when it does not occur.
        /// </summary>
        /// <typeparam name="T">Exception type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="Delegate">Delegate that returns a value</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        /// <returns>Returns the value returned by the delegate or thw appropriate exception</returns>
        public static R DoesNotThrow<T, R>(ReturnObjectDelegate<R> Delegate, string UserFailedMessage = "Assert.DoesNotThrow<T,R>() Failed")
        {
            try
            {
                return Delegate();
            }
            catch (Exception e)
            {
                if (e is T)
                    throw new DoesNotThrow(typeof(T), e, UserFailedMessage);
            }
            return default(R);
        }

        #endregion

        #region Empty

        /// <summary>
        /// Determines if an IEnumerable is empty or not
        /// </summary>
        /// <param name="Collection">Collection to check</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Empty(IEnumerable Collection, string UserFailedMessage = "Assert.Empty() Failed")
        {
            if (Collection == null)
                throw new ArgumentNullException("Collection");
            foreach (object Object in Collection)
                throw new NotEmpty(Collection,UserFailedMessage);
        }

        #endregion

        #region Equal

        /// <summary>
        /// Determines if two objects are equal
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Expected">Expected result</param>
        /// <param name="Result">Actual result</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Equal<T>(T Expected, T Result,string UserFailedMessage="Assert.Equal() Failed")
        {
            Equal(Expected, Result, new InternalClasses.EqualityComparer<T>(), UserFailedMessage);
        }

        /// <summary>
        /// Determines if two objects are equal
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Expected">Expected result</param>
        /// <param name="Result">Actual result</param>
        /// <param name="Comparer">Comparer used to compare the objects</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Equal<T>(T Expected, T Result, IEqualityComparer<T> Comparer, string UserFailedMessage = "Assert.Equal() Failed")
        {
            if (!Comparer.Equals(Expected, Result))
                throw new NotEqual(Expected, Result, UserFailedMessage);
        }

        #endregion

        #region Fail

        /// <summary>
        /// Called to generate failed tests
        /// </summary>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Fail(string UserFailedMessage = "Assert.Fail() Called")
        {
            throw new Fail(UserFailedMessage);
        }

        #endregion

        #region False

        /// <summary>
        /// Determins if something is false
        /// </summary>
        /// <param name="Value">Value</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void False(bool Value, string UserFailedMessage = "Assert.False() Failed")
        {
            if (Value)
                throw new NotFalse(UserFailedMessage);
        }

        #endregion

        #region Match

        /// <summary>
        /// Determins if a value is a regex match
        /// </summary>
        /// <param name="ExpectedRegex">Expected regex value</param>
        /// <param name="Result">Actual result</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Match(string ExpectedRegex,string Result, string UserFailedMessage = "Assert.Match() Failed")
        {
            if (!Regex.IsMatch(Result, ExpectedRegex))
                throw new NotMatch(ExpectedRegex, Result, UserFailedMessage);
        }

        #endregion

        #region NaN

        /// <summary>
        /// Determines if an object is NaN
        /// </summary>
        /// <param name="Object">Object to check</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NaN(double Object, string UserFailedMessage = "Assert.NaN() Failed")
        {
            if (!double.IsNaN(Object))
                throw new NotNaN(Object, UserFailedMessage);
        }

        #endregion

        #region NotBetween

        /// <summary>
        /// Determines if a value is not between two other values
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Result">Result</param>
        /// <param name="Low">Low value (inclusive)</param>
        /// <param name="High">High value (inclusive)</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NotBetween<T>(T Result, T Low, T High,
            string UserFailedMessage = "Assert.NotBetween() Failed") where T : IComparable
        {
            NotBetween(Result, Low, High, new InternalClasses.Comparer<T>());
        }

        /// <summary>
        /// Determines if a value is not between two other values
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Result">Result</param>
        /// <param name="Low">Low value (inclusive)</param>
        /// <param name="High">High value (inclusive)</param>
        /// <param name="Comparer">Compares the values</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NotBetween<T>(T Result, T Low, T High, IComparer<T> Comparer,
            string UserFailedMessage = "Assert.NotBetween() Failed") where T : IComparable
        {
            if (Comparer.Compare(High, Result) >= 0 || Comparer.Compare(Result, Low) >= 0)
                throw new Between(Result, Low, High, UserFailedMessage);
        }

        #endregion

        #region NotEmpty

        /// <summary>
        /// Determines if an IEnumerable is not empty
        /// </summary>
        /// <param name="Collection">Collection to check</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NotEmpty(IEnumerable Collection, string UserFailedMessage = "Assert.NotEmpty() Failed")
        {
            if (Collection == null)
                throw new ArgumentNullException("Collection");
            foreach (object Object in Collection)
                return;
            throw new Empty(Collection, UserFailedMessage);
        }

        #endregion

        #region NotEqual

        /// <summary>
        /// Determines if two objects are not equal
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Expected">Expected result</param>
        /// <param name="Result">Actual result</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NotEqual<T>(T Expected, T Result, string UserFailedMessage = "Assert.NotEqual() Failed")
        {
            NotEqual(Expected, Result, new InternalClasses.EqualityComparer<T>(), UserFailedMessage);
        }

        /// <summary>
        /// Determines if two objects are not equal
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Expected">Expected result</param>
        /// <param name="Result">Actual result</param>
        /// <param name="Comparer">Comparer used to compare the objects</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NotEqual<T>(T Expected, T Result, IEqualityComparer<T> Comparer, string UserFailedMessage = "Assert.NotEqual() Failed")
        {
            if(Comparer.Equals(Expected,Result))
                throw new Equal(Expected, Result, UserFailedMessage);
        }

        #endregion

        #region NotMatch

        /// <summary>
        /// Determins if a value is not a regex match
        /// </summary>
        /// <param name="ExpectedRegex">Expected regex value</param>
        /// <param name="Result">Actual result</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NotMatch(string ExpectedRegex, string Result, string UserFailedMessage = "Assert.NotMatch() Failed")
        {
            if (Regex.IsMatch(Result, ExpectedRegex))
                throw new MoonUnit.Exceptions.Match(ExpectedRegex, Result, UserFailedMessage);
        }

        #endregion

        #region NotNaN

        /// <summary>
        /// Determines if an object is not NaN
        /// </summary>
        /// <param name="Object">Object to check</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NotNaN(double Object, string UserFailedMessage = "Assert.NotNaN() Failed")
        {
            if (double.IsNaN(Object))
                throw new NaN(UserFailedMessage);
        }

        #endregion

        #region NotNull

        /// <summary>
        /// Determines if an object is not null
        /// </summary>
        /// <param name="Object">Object to check</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NotNull(object Object, string UserFailedMessage = "Assert.NotNull() Failed")
        {
            if (Object == null)
                throw new NullException(UserFailedMessage);
        }

        #endregion

        #region NotOfType

        /// <summary>
        /// Determines if an object is not of a specific type
        /// </summary>
        /// <param name="Object">Object to check</param>
        /// <typeparam name="T">Object type not expected</typeparam>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NotOfType<T>(object Object, string UserFailedMessage = "Assert.NotOfType() Failed")
        {
            NotOfType(Object, typeof(T), UserFailedMessage);
        }

        /// <summary>
        /// Determines if an object is not of a specific type
        /// </summary>
        /// <param name="Object">Object to check</param>
        /// <param name="Type">Object type not expected</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NotOfType(object Object, Type Type, string UserFailedMessage= "Assert.NotOfType() Failed")
        {
            if (Object.IsOfType(Type))
                throw new OfType(Type, Object.GetType(), UserFailedMessage);
        }

        #endregion

        #region NotSame

        /// <summary>
        /// Determines if an object is not the same instance as another object
        /// </summary>
        /// <param name="Expected">Expected value</param>
        /// <param name="Result">Actual value</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void NotSame(object Expected, object Result, string UserFailedMessage = "Assert.NotSame() Failed")
        {
            if (Object.ReferenceEquals(Expected, Result))
                throw new Same(Expected, Result, UserFailedMessage);
        }

        #endregion

        #region Null

        /// <summary>
        /// Determines if an object is null
        /// </summary>
        /// <param name="Object">Object to check</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Null(object Object, string UserFailedMessage = "Assert.Null() Failed")
        {
            if (Object != null)
                throw new NotNullException(Object, UserFailedMessage);
        }

        #endregion

        #region OfType

        /// <summary>
        /// Determines if an object is  of a specific type
        /// </summary>
        /// <param name="Object">Object to check</param>
        /// <typeparam name="T">Object type  expected</typeparam>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void OfType<T>(object Object, string UserFailedMessage = "Assert.OfType() Failed")
        {
            OfType(Object, typeof(T), UserFailedMessage);
        }

        /// <summary>
        /// Determines if an object is  of a specific type
        /// </summary>
        /// <param name="Object">Object to check</param>
        /// <param name="Type">Object type expected</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void OfType(object Object, Type Type, string UserFailedMessage = "Assert.OfType() Failed")
        {
            if (!Object.IsOfType(Type))
                throw new NotOfType(Type, Object.GetType(), UserFailedMessage);
        }

        #endregion

        #region Same

        /// <summary>
        /// Determines if an object is the same instance as another object
        /// </summary>
        /// <param name="Expected">Expected value</param>
        /// <param name="Result">Actual value</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Same(object Expected, object Result, string UserFailedMessage = "Assert.Same() Failed")
        {
            if (!Object.ReferenceEquals(Expected, Result))
                throw new NotSame(Expected, Result, UserFailedMessage);
        }

        #endregion

        #region Throws

        /// <summary>
        /// Used when a specific type of exception is expected
        /// </summary>
        /// <typeparam name="T">Type of the exception</typeparam>
        /// <param name="Delegate">Delegate called to test</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void Throws<T>(VoidDelegate Delegate, string UserFailedMessage = "Assert.Throws<T>() Failed")
        {
            bool Exception = false;
            try
            {
                Delegate();
            }
            catch (Exception e)
            {
                if (!(e is T))
                    throw new Throws(typeof(T), e, UserFailedMessage);
                Exception = true;
            }
            if (!Exception)
                throw new Throws(typeof(T), null, UserFailedMessage);
        }

        /// <summary>
        /// Used when a specific type of exception is expected (or a return value is expected when it does not occur)
        /// </summary>
        /// <typeparam name="T">Exception type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="Delegate">Delegate that returns a value</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        /// <returns>Returns the value returned by the delegate or thw appropriate exception</returns>
        public static R Throws<T, R>(ReturnObjectDelegate<R> Delegate, string UserFailedMessage = "Assert.Throws<T,R>() Failed")
        {
            bool Exception = false;
            try
            {
                return Delegate();
            }
            catch (Exception e)
            {
                if (!(e is T))
                    throw new Throws(typeof(T), e, UserFailedMessage);
                Exception = true;
            }
            if (!Exception)
                throw new Throws(typeof(T), null, UserFailedMessage);
            return default(R);
        }

        #endregion

        #region True

        /// <summary>
        /// Determins if something is true
        /// </summary>
        /// <param name="Value">Value</param>
        /// <param name="UserFailedMessage">Message passed to the output in the case of a failed test</param>
        public static void True(bool Value, string UserFailedMessage = "Assert.True() Failed")
        {
            if (!Value)
                throw new NotTrue(UserFailedMessage);
        }

        #endregion

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate that does not return a value
        /// </summary>
        public delegate void VoidDelegate();

        /// <summary>
        /// Delegate that returns a value
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <returns>The returned value</returns>
        public delegate T ReturnObjectDelegate<T>();

        #endregion
    }
}