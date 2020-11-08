using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Valis.Core.UnitTests
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class SimpleBaseClass
    {


        protected void TestSerialization<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            //Κάνουμε serialization της λίστα μας
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, item);
            var bytes = stream.GetBuffer();
            //Κάνουμε unserialization την λίστα μας
            MemoryStream stream2 = new MemoryStream(bytes);
            var _item = (T)formatter.Deserialize(stream2);

            //Ελέγχουμε την αρχική μας λίστα με την unserialized λίστα
            Assert.AreEqual<T>(item, _item);
            Assert.AreNotSame(item, _item);
        }


        protected Action<Action> _EXECUTEAndCATCH = delegate(Action foo)
        {
            #region
            bool _failed = false;
            try
            {
                foo();
            }
            catch
            {
                _failed = true;
            }
            Assert.IsTrue(_failed);
            #endregion
        };
        protected Action<Action, Type> _EXECUTEAndCATCHType = delegate(Action foo, Type exceptionType)
        {
            #region
            bool _failed = false;
            try
            {
                foo();
            }
            catch (Exception ex)
            {
                var _exType = ex.GetType();

                if (_exType.IsSubclassOf(exceptionType) || _exType == exceptionType)
                {
                    _failed = true;
                }
                else
                {
                    throw;
                }
            }
            Assert.IsTrue(_failed);
            #endregion
        };
        protected Action<Action> _EXECUTE_SUCCESS = delegate(Action foo)
        {
            #region
            bool _failed = false;
            try
            {
                foo();
            }
            catch
            {
                throw;
            }
            Assert.IsFalse(_failed);
            #endregion
        };
    }
}
