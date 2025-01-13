using Autofac.Extras.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class TestBase
    {
        private readonly AutoMock _mock;
        public TestBase()
        {
            _mock = AutoMock.GetLoose();
        }
        public T Create<T>()
        {
            return _mock.Create<T>();
        }

        public Mock<T> CreateMock<T>() where T : class
        {
            return _mock.Mock<T>();
        }
    }
}
