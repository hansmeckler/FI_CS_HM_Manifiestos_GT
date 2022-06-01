using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Manifiesto.Data.Maritimo;
using Manifiesto.Data.Admin;

namespace Manifiesto.Web.Tests.Maritimo
{
    /// <summary>
    /// Summary description for ManifestControllerTest
    /// </summary>
    [TestClass]
    public class ManifestControllerTest
    {

        [TestMethod]
        public void Index_Return_NotNull()
        {
            //
            // TODO: Add test logic here
            //
            var mock_manifest = new Mock<IMaritimoServices>();
            var mock_admin = new Mock<IAdminService>();

           
        }
    }
}
