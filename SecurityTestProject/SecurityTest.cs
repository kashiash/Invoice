using BusinessObjectsLibrary.BusinessObjects;
using DatabaseUpdater;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestProject
{
    public class SecurityTests
    {
        [Test]
        public void SecurityTest()
        {
            // ## Step 0. Preparation. Create or update database
            string connectionString = "Integrated Security=SSPI;Pooling=false;Data Source=.;Initial Catalog=Invoice5";
            CreateDemoData(connectionString);

            // ## Step 1. Initialization. Create a Secured Data Store and Set Authentication Options
            AuthenticationStandard authentication = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
            security.RegisterXPOAdapterProviders();
            SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
            RegisterEntities(objectSpaceProvider);

            // ## Step 2. Authentication. Log in as a 'User' with an Empty Password
            authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName: "User", password: string.Empty));
            IObjectSpace loginObjectSpace = objectSpaceProvider.CreateObjectSpace();
            security.Logon(loginObjectSpace);

            // ## Step 3. Authorization. Access and Manipulate Data/UI Based on User/Role Rights
            Console.WriteLine($"{"Full Name",-40}{"Department",-40}");
            using (IObjectSpace securedObjectSpace = objectSpaceProvider.CreateObjectSpace())
            {
                // User cannot read protected entities like PermissionPolicyRole.
                Debug.Assert(securedObjectSpace.GetObjects<PermissionPolicyRole>().Count == 0);
                foreach (Employee employee in securedObjectSpace.GetObjects<Employee>())
                { // User can read Employee data.
                  // User can read Department data by criteria.
                    bool canRead = security.CanRead(securedObjectSpace, employee, memberName: nameof(Employee.Department));
                    Debug.Assert(!canRead == (employee.Department == null));
                    // Mask protected property values when User has no 'Read' permission.
                    var department = canRead ? employee.Department.Title : "*******";
                    Console.WriteLine($"{employee.FullName,-40}{department,-40}");

                }
            }
            static void RegisterEntities(IObjectSpaceProvider objectSpaceProvider)
            {
                objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Employee));
                objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
                objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
            }
            static void CreateDemoData(string connectionString)
            {
                using (var objectSpaceProvider = new XPObjectSpaceProvider(connectionString))
                {
                    RegisterEntities(objectSpaceProvider);
                    using (var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true))
                    {
                        new UpdaterDepartment(objectSpace).UpdateDatabase();
                    }
                }
            }
        }
    }
}
