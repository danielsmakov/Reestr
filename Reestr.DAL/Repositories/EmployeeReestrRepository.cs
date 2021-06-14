using Dapper;
using Reestr.DAL.Entities;
using Reestr.DAL.Interfaces;
using Reestr.DAL.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Repositories
{
    class EmployeeReestrRepository : IRepository<EmployeeReestr>
    {
        string connectString = ConfigurationManager.ConnectionStrings["RegistryDBConnection"].ConnectionString;
        public EmployeeReestr Get(int id)
        {
            using (SqlConnection _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    EmployeeReestr entity = _con.QuerySingle<EmployeeReestr>("Select * From EmployeeReestr Where Id = @Id",
                        new
                        {
                            id = id
                        }
                        );
                    _con.Close();
                    return entity;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public List<EmployeeReestr> List(OrganizationQuery query)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();

                    var where = "WHERE 1=1";
                    if (query.IsDeleted) where += " AND EndDate is not null ";
                    if (!string.IsNullOrEmpty(query.Name)) where += " AND Name like '%@Name%'";

                    List<EmployeeReestr> orgs = _con.Query<EmployeeReestr>($"Select * From Organizations {where}",
                        new
                        {
                            name = query.Name
                        }
                        ).ToList();
                    _con.Close();
                    return orgs;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Insert(EmployeeReestr entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("Insert Into EmployeeReestr (Id, OrganizationId, IIN, FullName, DateOfBirth, PhoneNumber, BeginDate, EndDate) Values (@Id, @OrganizationId, @IIN, @FullName, @DateOfBirth @PhoneNumber, @BeginDate, @EndDate)", new { entity });
                    _con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Update(EmployeeReestr entity)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("Update EmployeeReestr Set OrganizationId = @OrganizationId, IIN = @IIN,, FullName = @FullName, DateOfBirth = @DateOfBirth, PhoneNumber = @PhoneNumber, BeginDate = @BeginDate Where Id = @Id", new { entity });
                    _con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Delete(int id)
        {
            using (var _con = new SqlConnection(connectString))
            {
                try
                {
                    _con.Open();
                    _con.Execute("Update EmployeeReestr Set EndDate = GETDATE() Where Id = @Id", new { id });
                    _con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
