using Moq;
using MVCContactRecords.Data;
using MVCContactRecords.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCContactRecordsTests.Controllers
{
    public class ContactControllerTests
    {
        [Fact]
        public void Index_ReturnsListOfContact_WhenContactExists()
        {
            //Arrange
            var mockAppDbContext = new Mock<AppDbContext>();
            var mockContactService = new Mock<IContactService>();


        }
    }
}
